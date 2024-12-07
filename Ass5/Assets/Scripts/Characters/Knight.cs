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

    private float damageReduced; // Damage reduced when blocking

    protected override void Awake()
    {
        base.Awake();

        ability = Stats.GetInstantiatedAbility() as GuardPoint;
        ability.Initialize(this);

        shieldMaxEndurance = 10;
        shieldEndurance = shieldMaxEndurance;
        shieldCooldown = 10;
        timeSinceShieldBreak = 0;
        isBlocking = false;
        timeSinceBlock = 0;
        timeForGuardPoint = 0.5f;
    }

    protected override void Update()
    {
        timeSinceBlock += Time.deltaTime;
        base.Update();
        if ((timeSinceBlock <= timeForGuardPoint) & !ability.abilityIsActivated)
            ability.Activate();
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
        }
        else
            isBlocking = false;
    }

    public override void Attack()
    {
        base.Attack();
        isBlocking = false;
    }

    public override void TakeDamage(float damage)
    {
        float newDamge = damage;
        if (isBlocking)
        {
            newDamge = damage * (1 - damageReduced);
            ReduceShieldEndurance(damage * damageReduced);
        }
        base.TakeDamage(newDamge);
    }

    private void Block()
    {
        isBlocking = true;
        Debug.Log("Blocking");
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
