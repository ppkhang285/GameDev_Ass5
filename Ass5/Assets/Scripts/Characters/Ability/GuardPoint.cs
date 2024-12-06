using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/GuardPoint")]
public class GuardPoint : Ability
{
    private float shieldMaxEndurance;
    private float shieldEndurance;

    private float shieldCooldown;
    private float timeSinceShieldBreak;

    private float attackBuff;
    private float damageReduced; // Damage reduced when blocking

    private bool isBlocking;
    private float timeSinceBlock;
    private float timeForGuardPoint;

    public GuardPoint(Character character) : base(character, "Guard Point", 5) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        shieldMaxEndurance = 10;
        shieldEndurance = 10;
        shieldCooldown = 10;
        timeSinceShieldBreak = 0;
        attackBuff = 1.2f;
        damageReduced = 0.1f;
        isBlocking = false;
        timeSinceBlock = 0;
        timeForGuardPoint = 0.5f;
    }

    public override bool CheckActivateCondition()
    {
        return (timeSinceBlock <= timeForGuardPoint) & !abilityIsActivated;
    }

    public override void Activate()
    {
        base.Activate();
        shieldEndurance = shieldMaxEndurance;
        timeSinceBlock = timeForGuardPoint;
        character.CurrentDamage *= attackBuff;
        character.Resistence = 1;
    }

    public override void Passive()
    {
        base.Passive();
        // Other passive mechanics of the character
        UpdateResistence();
        HandleBlocking();
        RecoverShield();
    }

    public override void Attack()
    {
        base.Attack();
        isBlocking = false;
    }

    private void UpdateResistence()
    {
        if (isBlocking)
            character.Resistence = 1 - (1 - character.Stats.resistence) * (1 - damageReduced);
        else
            character.Resistence = character.Stats.resistence;
    }

    private void HandleBlocking()
    {
        timeSinceBlock += Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && shieldEndurance > 0) // RMB pressed and shield is usable
        {
            isBlocking = true;
            timeSinceBlock = 0;
        }
        else
            isBlocking = false;
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

    public override void TakeDamage(float damage)
    {
        ReduceShieldEndurance(damage);
    }
}