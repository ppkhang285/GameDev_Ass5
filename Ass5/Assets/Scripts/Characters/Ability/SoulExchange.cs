using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SoulExchange")]
public class SoulExchange : Ability
{
    private float manaPerCast;
    private float currentMana;
    private float hpPerCast;
    public float manaRefill;

    private float timeForSalvation = 10;

    private float attackBuff = 1.5f;
    private float attackRangeBuff = 1.5f;

    public Salvation salvation;
    private Salvation salvationAbility;

    public SoulExchange(Character character) : base(character, "Soul Exchange", 10) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        if (salvation != null)
        {
            salvationAbility = Instantiate(salvation);
            salvationAbility.Initialize(character);
        }
        else
            salvationAbility = null;
    }

    public override void Activate()
    {
        base.Activate();
        character.CurrentDamage *= attackBuff;
        character.AttackRange *= attackRangeBuff;
        // Debug.Log("Ability " + AbilityName + " lasts for " + duration + " seconds");
    }

    public override void Passive()
    {
        if (Input.GetKeyDown(KeyCode.F) && !salvationAbility.abilityIsActivated) // Press F to switch on/off soul exchange mode if not in salvation mode
        {
            if (abilityIsActivated)
                Deactivate();
            else
                Activate();
        }

        // Other passive mechanics of the character
        RefillMana();
        if (abilityIsActivated)
            InSoulExchange();
        else if (salvationAbility.abilityIsActivated)
            salvationAbility.Passive();
    }

    public override void Attack()
    {
        if (abilityIsActivated) // In soul exchange mode
        {
            base.Attack();
            character.CurrentHP -= hpPerCast;
        }
        else
        {
            if (currentMana >= manaPerCast) // Enough mana to cast
            {
                base.Attack();
                currentMana -= manaPerCast;
            }
        }
    }

    private void InSoulExchange()
    {
        timeSinceActivate += Time.deltaTime;
        if (timeSinceActivate >= timeForSalvation) // Has been in soul exchange mode for enough time
        {
            Deactivate();
            salvationAbility.abilityIsActivated = true;
        }
    }

    private void RefillMana()
    {
        currentMana += manaRefill;
    }
}
