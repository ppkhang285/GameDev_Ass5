using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Salvation")]
public class Salvation : Ability
{
    private float healAmount;
    private float manaRefillBuff = 1.2f;
    private float speedBuff = 1.2f;
    private float damageReduce = 0.1f;

    private SoulExchange soulExchange;
    public Salvation(Character character) : base(character, "Salvation", 10) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        soulExchange = (SoulExchange)character.ability;
    }

    public override void Activate()
    {
        base.Activate();
        character.MovementSpeed *= speedBuff;
        character.Resistence = 1 - (1 - character.Resistence) * (1 - damageReduce);
        soulExchange.manaRefill *= manaRefillBuff;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        soulExchange.manaRefill /= manaRefillBuff;
    }

    public override void Passive()
    {
        base.Passive();

        // Other passive mechanics of the character
        Heal();
    }

    private void Heal()
    {
        character.CurrentHP += healAmount;
    }
}
