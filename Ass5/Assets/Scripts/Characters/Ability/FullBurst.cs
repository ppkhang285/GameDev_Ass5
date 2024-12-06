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

    public FullBurst() : base("Full Burst", 10, 10) { }

    public override bool CheckActivateCondition()
    {
        return rage >= maxRage;
    }

    public override void Activate(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        character.CurrentDamage *= attackBuff;
        character.AttackSpeed *= attackSpeedBuff;
        character.MovementSpeed *= speedBuff;
        character.Resistence *= resistenceDebuff;
    }

    public override void Deactivate(GameObject player)
    {
        base.Deactivate(player);
        Rage = 0;
    }

    public override void Passive(GameObject player)
    {
        base.Passive(player);

        // Other passive mechanics of the character
        DecreaseRage();
    }

    public override void Attack(GameObject player)
    {
        base.Attack(player);
        if (hitTarget) // Just hit an anemy
            Rage += rageIncreasePerHit;
    }

    private void DecreaseRage() // Rage decreases when not attacking
    {
        Rage -= rageDecreaseSpeed;
    }
}
