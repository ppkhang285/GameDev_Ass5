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

    public bool isSalvation = false;

    private SoulExchange soulExchange;
    public Salvation() : base("Salvation", 10) { }

    public void Initialize(SoulExchange soulExchange)
    {
        this.soulExchange = soulExchange;
    }

    public override bool CheckActivateCondition()
    {
        return isSalvation;
    }

    public override void Activate(GameObject player)
    {
        isSalvation = true;
        Character character = player.GetComponent<Character>();
        character.MovementSpeed *= speedBuff;
        character.Resistence = 1 - (1 - character.Resistence) * (1 - damageReduce);
        soulExchange.manaRefill *= manaRefillBuff;
    }

    public override void Deactivate(GameObject player)
    {
        base.Deactivate(player);
        isSalvation = false;
        soulExchange.manaRefill /= manaRefillBuff;
    }

    public override void Passive(GameObject player)
    {
        base.Passive(player);

        // Other passive mechanics of the character
        Heal(player);
    }

    private void Heal(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        character.CurrentHP += healAmount;
    }
}
