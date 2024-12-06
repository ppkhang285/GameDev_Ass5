using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/GuardPoint")]
public class GuardPoint : Ability
{
    private float shieldMaxEndurance = 10;
    private float shieldEndurance = 10;

    private float shieldCooldown = 10;
    private float timeSinceShieldBreak = 0;

    private float attackBuff = 1.2f;
    private float damageReduced = 0.1f; // Damage reduced when blocking
    private bool isBlocking = false;

    public GuardPoint() : base("Guard Point", 5) { }

    public override bool CheckActivateCondition()
    {
        // TODO: add checking for activation condition
        return true;
    }

    public override void Activate(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        shieldEndurance = shieldMaxEndurance;
        character.CurrentDamage *= attackBuff;
        character.Resistence = 1;
    }

    public override void Passive(GameObject player)
    {
        base.Passive(player);

        // Other passive mechanics of the character
        UpdateResistence(player);
        HandleBlocking(player);
        RecoverShield();
    }

    public override void Attack(GameObject player)
    {
        base.Attack(player);
        isBlocking = false;
    }

    private void UpdateResistence(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        if (isBlocking)
            character.Resistence = 1 - (1 - character.Stats.resistence) * (1 - damageReduced);
        else
            character.Resistence = character.Stats.resistence;
    }

    private void HandleBlocking(GameObject player)
    {
        if (Input.GetMouseButton(1) && shieldEndurance > 0) // RMB pressed and shield is usable
        {
            isBlocking = true;
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

    public void ReduceShieldEndurance(float amount)
    {
        shieldEndurance -= amount;
        if (shieldEndurance <= 0)
            timeSinceShieldBreak = 0; // Shield breaks, start shield cooldown
    }
}