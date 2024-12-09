using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Class for playable characters
public class Character : MonoBehaviour
{
    public Animator animator { get; private set; }

    [SerializeField]
    protected CharacterStats characterStats;
    public CharacterStats Stats;
    private PhotonView photonView;
    private float hp;

    private Rigidbody rb;
    public float CurrentHP 
    { 
        get { return hp; }
        set 
        {
            if (value < 0) hp = 0;
            else if (value > Stats.hp) hp = Stats.hp;
            else hp = value;
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
            if (value < 0) resistence = 0;
            else if (value > 1) resistence = 1;
            else resistence = value;
        }
    }

    public float TimeSinceLastAttack;
    private bool isDead;

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
        if (isDead)
            return;
        TimeSinceLastAttack += Time.deltaTime;
        if (ability != null)
            ability.Passive();
        if (GameManager.Instance.isPvP)
        {
            if (photonView.IsMine)
                HandleInput();
        }
        else
            HandleInput();
    }

    protected virtual void HandleInput()
    {
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
        TimeSinceLastAttack = 0;
        animator.SetTrigger("attack");
    }

    public virtual void TakeDamage(float damage)
    {
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

    public void ResetStats()
    {
        CurrentDamage = Stats.damage;
        Resistence = Stats.resistence;
        Speed = Stats.speed;
        AttackRange = Stats.attackRange;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
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
        } else if (other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.OnPickup(this);
        }
    }
}
