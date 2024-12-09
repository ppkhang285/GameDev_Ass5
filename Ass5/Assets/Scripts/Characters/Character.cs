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
        if (photonView.IsMine)
        {
            HandleInput();
        }
    }

    protected virtual void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Move(horizontal, vertical);
        if (Input.GetMouseButtonDown(0) && TimeSinceLastAttack >= AttackCooldown)
            Attack();
    }

    public virtual void Move(float horizontal, float vertical)
    {
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        animator.SetFloat("speed", direction.magnitude);
        transform.Translate(direction * Speed * Time.deltaTime, Space.Self);
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
            Character damager = damagerObject.GetComponent<Character>();
            TakeDamage(damager.CurrentDamage);
            if (damager.Stats.charType == CharacterType.Berserker)
            {
                (damager as Berserker).HitTarget();
            }
        } else if (other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.OnPickup(this);
        }
    }
}
