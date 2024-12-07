using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for playable characters
public class Character : MonoBehaviour
{
    public Animator animator { get; private set; }

    [SerializeField]
    protected CharacterStats characterStats;
    public CharacterStats Stats;

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
    public float AttackSpeed;
    public float MovementSpeed;
    public float AttackRange;
    public float AttackCooldown;
    public Ability ability;

    private float resistence;
    public float Resistence // Percentage of damage reduced, 0 = receive full damage, 1 = ignore all damage
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

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        Stats = Instantiate(characterStats);

        ability = Stats.GetInstantiatedAbility();
        ability.Initialize(this);

        CurrentHP = Stats.hp;
        CurrentDamage = Stats.damage;
        Resistence = Stats.resistence;
        AttackSpeed = Stats.attackSpeed;
        MovementSpeed = Stats.movementSpeed;
        AttackRange = Stats.attackRange;
        AttackCooldown = Stats.attackCooldown;
        TimeSinceLastAttack = AttackCooldown;
    }

    protected virtual void Update()
    {
        TimeSinceLastAttack += Time.deltaTime;
        if (ability != null)
            ability.Passive();
        HandleInput();
    }

    protected virtual void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isRunning", true);
            Move(horizontal, vertical);
        }
        else
            animator.SetBool("isRunning", false);

        if (Input.GetMouseButtonDown(0) && TimeSinceLastAttack >= AttackCooldown)
            Attack();
    }

    public virtual void Move(float horizontal, float vertical)
    {
        Vector3 direction = new Vector3(horizontal, 0, vertical) * MovementSpeed * Time.deltaTime;
        transform.Translate(direction, Space.Self);
    }

    public virtual void Attack()
    {
        TimeSinceLastAttack = 0;
        animator.SetTrigger("attack");
    }

    public virtual void TakeDamage(float damage)
    {
        animator.SetTrigger("hit");
        CurrentHP -= damage * (1 - resistence);
        if (CurrentHP <= 0)
            Die();
    }

    public virtual void Die()
    {
        
    }

    public void ResetStats()
    {
        CurrentDamage = Stats.damage;
        Resistence = Stats.resistence;
        AttackSpeed = Stats.attackSpeed;
        MovementSpeed = Stats.movementSpeed;
        AttackRange = Stats.attackRange;
    }
}
