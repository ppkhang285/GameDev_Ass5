using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Animator animator { get; private set; }

    [SerializeField]
    private CharacterStats characterStats;
    public CharacterStats Stats
    {
        get { return characterStats; }
    }

    public float CurrentHP { get; set; }
    public float CurrentDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float MovementSpeed { get; set; }

    private float resistence;
    public float Resistence // Percentage of damage received, 0 = receive full damage, 1 = ignore all damage
    {
        get { return resistence;  }
        set
        {
            if (value < 0) resistence = 0;
            else if (value > 1) resistence = 1;
            else resistence = value;
        }
    } 

    void Start()
    {
        animator = GetComponent<Animator>();

        CurrentHP = characterStats.hp;
        CurrentDamage = characterStats.damage;
        Resistence = characterStats.resistence;
        AttackSpeed = characterStats.attackSpeed;
        MovementSpeed = characterStats.movementSpeed;
    }

    void Update()
    {
        TestAnim();
    }


    public void TestAnim()
    {
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
    }

    private void TakeDamage(float damage)
    {
        animator.SetTrigger("hit");
        CurrentHP -= damage * (1 - resistence);
        if (CurrentHP <= 0)
            Die();
    }

    private void Die()
    {

    }
}
