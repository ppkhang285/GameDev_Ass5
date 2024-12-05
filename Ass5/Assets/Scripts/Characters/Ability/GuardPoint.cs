using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/GuardPoint")]
public class GuardPoint : Ability
{
    private float shieldMaxEndurance = 10;
    private float shieldEndurance = 10;

    private float shieldCooldown = 10;
    private float timeSinceShieldBreak;

    private float duration = 10;
    private float timeSinceActivate;

    private float attackBuff;
    public float damageReduced;

    public GuardPoint() : base("Guard Point") { }

    public override void Activate(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        if (timeSinceActivate <= duration) // Ability still has effect
        {
            if (timeSinceActivate <= 0) // First call when ability is activated
            {
                shieldEndurance = shieldMaxEndurance;
                character.CurrentDamage *= attackBuff;
                character.Resistence = 1;
            }
            timeSinceActivate += Time.deltaTime;
        } 
        else
        {
            // Return player to normal state
            character.CurrentDamage = character.Stats.damage;
            character.Resistence = character.Stats.resistence;
            timeSinceActivate = 0;
        }

        // Other passive mechanics of the character
        HandleBlocking(player);
        RecoverShield();
    }

    private void HandleBlocking(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        if (shieldEndurance > 0) // Shield is usable
        {
            if (Input.GetMouseButtonDown(1))
                character.Resistence = character.Resistence + damageReduced;  
            else
                character.Resistence = character.Stats.resistence;
        } 
        else
            character.Resistence = character.Stats.resistence;
    }

    private void RecoverShield()
    {
        if (timeSinceShieldBreak >= shieldCooldown && shieldEndurance <= 0) // Recover shield if shield not usable
        {
            timeSinceShieldBreak = 0;
            shieldEndurance = shieldMaxEndurance;
        }
        else
        {
            timeSinceShieldBreak += Time.deltaTime;
        }
    }

    public void ReduceShieldEndurance(float amount)
    {
        shieldEndurance -= amount;
        if (shieldEndurance <= 0)
            timeSinceShieldBreak = shieldCooldown; // Start shield cooldown
    }
}