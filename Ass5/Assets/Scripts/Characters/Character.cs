using UnityEngine;
using Photon.Pun;

public class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator animator { get; private set; }

    [SerializeField]
    protected CharacterStats characterStats;
    public CharacterStats Stats;

    private PhotonView photonView;

    private Rigidbody rb;
    private float hp;
    public float CurrentHP 
    { 
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(value, 0, Stats.hp);

            if (GameManager.Instance.isPvP && photonView.IsMine)
                photonView.RPC("UpdateHP", RpcTarget.All, hp);
        }
    }

    public float CurrentDamage;
    public float Speed;
    public float AttackRange;
    public float AttackCooldown;
    public Ability ability;

    private float resistence;
    public float Resistence
    {
        get { return resistence; }
        set
        {
            resistence = Mathf.Clamp01(value);
        }
    }

    public float TimeSinceLastAttack;
    private bool isDead;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    protected virtual void Awake()
    {
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        Stats = Instantiate(characterStats);

        ability = Stats.GetInstantiatedAbility();
        ability.Initialize(this);

        CurrentHP = Stats.hp;
        CurrentDamage = Stats.damage;
        Resistence = Stats.resistence;
        Speed = Stats.speed;
        AttackRange = Stats.attackRange;
        AttackCooldown = Stats.attackCooldown;
        TimeSinceLastAttack = AttackCooldown;
        isDead = false;
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.isPvP)
        {
            if (photonView.IsMine)
                UpdateLocalPlayer();
            else
            {
                transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10);
                transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10);
            }
        }
        else
            UpdateLocalPlayer();
    }

    private void UpdateLocalPlayer()
    {
        if (isDead)
            return;

        TimeSinceLastAttack += Time.deltaTime;

        if (ability != null)
            ability.Passive();

        HandleInput();
    }

    protected virtual void HandleInput()
    {
        if (GameManager.Instance.isPvP && !photonView.IsMine)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Move(horizontal, vertical);

        if (Input.GetMouseButtonDown(0) && TimeSinceLastAttack >= AttackCooldown)
            Attack();

        //
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Rotate(mouseX);

    }

    public virtual void Rotate(float rotY) 
    {
        float rot = rotY;

        transform.Rotate(0, rot, 0);
    }

    public virtual void Move(float horizontal, float vertical)
    {
       // Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
        //transform.Translate(direction * Speed * Time.deltaTime, Space.Self);

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

       
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

       
        Vector3 direction = (cameraForward * vertical + cameraRight * horizontal).normalized;
        rb.MovePosition(rb.position + direction * Speed * Time.deltaTime);

        animator.SetFloat("speed", direction.magnitude);
    }

    public virtual void Attack()
    {
        if (GameManager.Instance.isPvP && photonView.IsMine)
            photonView.RPC("NetworkAttack", RpcTarget.All);

        TimeSinceLastAttack = 0;
        animator.SetTrigger("attack");
    }

    public virtual void TakeDamage(float damage)
    {
        if (GameManager.Instance.isPvP && photonView.IsMine)
            photonView.RPC("NetworkTakeDamage", RpcTarget.All, damage);

        animator.SetTrigger("hit");
        CurrentHP -= damage * (1 - Resistence);

        GameplayManager.Instance.hudManager.UpdateHpHUD(CurrentHP, Stats.hp);
        if (CurrentHP <= 0)
            Die();
    }

    public virtual void Die()
    {
        animator.SetTrigger("dead");
        isDead = true;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (GameManager.Instance.isPvP)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(CurrentHP);
            }
            else
            {
                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();
                CurrentHP = (float)stream.ReceiveNext();
            }
        }
    }

    public void ResetStats()
    {
        CurrentDamage = Stats.damage;
        Resistence = Stats.resistence;
        Speed = Stats.speed;
        AttackRange = Stats.attackRange;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isPvP && !photonView.IsMine)
            return;

        if (other.CompareTag("Missile"))
        {
            float damage = ArrowManager.Instance.NotifyArrowHit(other.gameObject);
            TakeDamage(damage);
        }
        else if (other.CompareTag("Melee"))
        {
            GameObject damagerObject = other.gameObject.transform.root.gameObject;
            if (damagerObject.GetComponent<Character>())
            {
                Character damager = damagerObject.GetComponent<Character>();
                TakeDamage(damager.CurrentDamage);

                if (damager.Stats.charType == CharacterType.Berserker)
                    (damager as Berserker).HitTarget();
            }
            else if (damagerObject.GetComponent<EnemyAI>())
            {
                EnemyAI damager = damagerObject.GetComponent<EnemyAI>();
                TakeDamage(damager.CurrentDamage);
            }
        }
        else if (other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.OnPickup(this);
        }
    }

    [PunRPC]
    public void UpdateHP(float newHP)
    {
        hp = newHP;
    }

    [PunRPC]
    public void NetworkAttack()
    {
        TimeSinceLastAttack = 0;
        animator.SetTrigger("attack");
    }

    [PunRPC]
    public void NetworkTakeDamage(float damage)
    {
        animator.SetTrigger("hit");
        CurrentHP -= damage * (1 - Resistence);
        GameplayManager.Instance.hudManager.UpdateHpHUD(CurrentHP, Stats.hp);
        if (CurrentHP <= 0)
            Die();
    }
}