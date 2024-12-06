using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for playable characters
public class Character : MonoBehaviour
{
    public Animator animator { get; private set; }

    [SerializeField]
    private CharacterStats characterStats;
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

    public float CurrentDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float MovementSpeed { get; set; }
    public float AttackRange { get; set; }
    public float AttackCooldown { get; set; }
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

    void Awake()
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

    void Update()
    {
        TestAnim();
        ability.Passive();
        Move();
        Attack();
    }


    public void TestAnim()
    {
        //animator.SetBool("isRunning", false);
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    animator.SetBool("isRunning", true);
        //}

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    animator.SetBool("isRunning", false);
        //}

        //if (Input.GetMouseButtonDown(0)) {
        //    animator.SetTrigger("attack");
        //}

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    animator.SetTrigger("hit");
        //}

        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetFloat("x", -1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetFloat("x", 1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetFloat("y", 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetFloat("y", -1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("dodge");
        }

    }

    public void ResetStats()
    {
        CurrentDamage = characterStats.damage;
        Resistence = characterStats.resistence;
        AttackSpeed = characterStats.attackSpeed;
        MovementSpeed = characterStats.movementSpeed;
        AttackRange = characterStats.attackRange;
    }


    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isRunning", true);
            ability.Move();
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void Attack()
    {
        TimeSinceLastAttack += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && TimeSinceLastAttack >= AttackCooldown)
        {
            ability.Attack();
        }       
    }

    public void TakeDamage(float damage)
    {
        animator.SetTrigger("hit");
        CurrentHP -= damage * (1 - resistence);
        ability.TakeDamage(damage);
        if (CurrentHP <= 0)
            Die();
    }

    private void Die()
    {

    }
}
