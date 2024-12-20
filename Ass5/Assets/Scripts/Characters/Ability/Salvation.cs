using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Salvation")]
public class Salvation : Ability
{
    private float healAmount;
    private float manaRefillBuff;
    private float speedBuff;
    private float damageReduced;

    private Wizard wizardCharacter;

    public Salvation(Wizard character) : base(character, "Salvation", 10) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        wizardCharacter = character as Wizard;

        healAmount = 0.1f; 
        manaRefillBuff = 1.2f;
        speedBuff = 1.2f;
        damageReduced = 0.1f;
    }

    public override void Activate()
    {
        base.Activate();
        wizardCharacter.Speed *= speedBuff;
        wizardCharacter.Resistence = 1 - (1 - character.Resistence) * (1 - damageReduced);
        wizardCharacter.manaRefill *= manaRefillBuff;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        wizardCharacter.manaRefill /= manaRefillBuff;
    }

    public override void Passive()
    {
        base.Passive();
        Heal();
    }

    private void Heal()
    {
        character.CurrentHP += healAmount;
    }
}
