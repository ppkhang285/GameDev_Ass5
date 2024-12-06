using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FullBurst")]
public class FullBurst : Ability
{
    private float maxRage = 100;
    private float rage;
    public float Rage
    {
        get { return rage; }
        set
        {
            if (value < 0) rage = 0;
            else if (value > 100) rage = 100;
            else rage = value;
        }
    }

    private float attackBuff = 1.5f;
    private float attackSpeedBuff = 1.2f;
    private float speedBuff = 1.2f;
    private float resistenceDebuff = 0.9f;

    private float rageIncreasePerHit = 10f;
    private float rageDecreaseSpeed = 0.2f;

    public FullBurst(Character character) : base(character, "Full Burst", 10) { }

    public override bool CheckActivateCondition()
    {
        return (rage >= maxRage) & !abilityIsActivated;
    }

    public override void Activate()
    {
        base.Activate();
        character.CurrentDamage *= attackBuff;
        character.AttackSpeed *= attackSpeedBuff;
        character.MovementSpeed *= speedBuff;
        character.Resistence *= resistenceDebuff;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Rage = 0;
    }

    public override void Passive()
    {
        base.Passive();

        // Other passive mechanics of the character
        DecreaseRage();
        BuffAttack();
    }

    public override void Attack()
    {
        base.Attack();
        if (hitTarget) // Just hit an anemy
            Rage += rageIncreasePerHit;
    }

    private void DecreaseRage() // Rage decreases when not attacking
    {
        Rage -= rageDecreaseSpeed;
    }

    private void BuffAttack()
    {
        if (!abilityIsActivated)
            character.CurrentDamage *= (1 + rage/100);
    }

}
