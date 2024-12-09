using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{
    private float shieldMaxEndurance; 
    private float shieldEndurance; 

    private float shieldCooldown; 
    private float timeSinceShieldBreak;

    private bool isBlocking; 
    public float timeSinceBlock;
    public float timeForGuardPoint; 

    private float shieldResistenceBuff; // Resistence buff when using shield to block
    private float shieldSpeedReduced; // Speed reduced when using shield to block

    protected override void Awake()
    {
        base.Awake();

        ability = Stats.GetInstantiatedAbility() as GuardPoint;
        ability.Initialize(this);

        shieldMaxEndurance = 25;
        shieldEndurance = shieldMaxEndurance;
        shieldCooldown = 8;
        timeSinceShieldBreak = 0;
        isBlocking = false;
        timeSinceBlock = 0;
        timeForGuardPoint = 0.5f;
        shieldResistenceBuff = 1f / 3;
        shieldSpeedReduced = 4f / 3;
    }

    protected override void Update()
    {
        timeSinceBlock += Time.deltaTime;
        base.Update();
        RecoverShield();
    }

    protected override void HandleInput()
    {
        base.HandleInput();
        if (shieldEndurance > 0) // RMB pressed and shield is usable
        {
            if (Input.GetMouseButtonDown(1))
            {
                timeSinceBlock = 0;
                Block();
            }
            else if (Input.GetMouseButton(1))
                Block();
            else
                Unblock();
        }
        else
            Unblock();
    }

    public override void Attack()
    {
        Unblock();
        base.Attack();
    }

    public override void TakeDamage(float damage)
    {
        float newDamage = damage;
        if (isBlocking)
        {
            newDamage = damage * (1 - shieldResistenceBuff);
            ReduceShieldEndurance(damage);
        }
        base.TakeDamage(newDamage);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile") || other.CompareTag("Melee"))
        {
            if ((timeSinceBlock <= timeForGuardPoint) & !ability.abilityIsActivated)
                ability.Activate();
        } 
    }

    private void Block()
    {
        isBlocking = true;
        AttackCooldown = Stats.attackCooldown * shieldSpeedReduced;
        Debug.Log("Blocking");
       // animator.Play("Block");
    }

    private void Unblock()
    {
        isBlocking = false;
        AttackCooldown = Stats.attackCooldown;
    }

    private void RecoverShield()
    {
        if (shieldEndurance <= 0) // If shield is not usable
        {
            if (timeSinceShieldBreak >= shieldCooldown) // Recover shield 
            {
                timeSinceShieldBreak = 0;
                shieldEndurance = shieldMaxEndurance;
            }
            else
                timeSinceShieldBreak += Time.deltaTime;
        }
    }

    private void ReduceShieldEndurance(float amount)
    {
        shieldEndurance -= amount;
        if (shieldEndurance <= 0)
            timeSinceShieldBreak = 0; // Shield breaks, start shield cooldown
    }
}
