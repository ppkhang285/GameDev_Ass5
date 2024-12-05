using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CharacterStats characterStats;

    private Animator animator;

    public CharacterStats stats
    {
        get { return characterStats; }
    }

    public float CurrentHP { get; set; }
    public float CurrentDamage { get; set; }

    void Start()
    {
        animator = GetComponent<Animator>();

        CurrentHP = characterStats.hp;
        CurrentDamage = characterStats.damage;
    }



    // Update is called once per frame
    void Update()
    {
        TestAnim();
    }


    public void TestAnim()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isRunning", true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("hit");
        }

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
   

}
