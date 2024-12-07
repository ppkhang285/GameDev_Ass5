using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SoulExchange")]
public class SoulExchange : Ability
{
    private float attackBuff;
    private float attackRangeBuff;

    public Salvation salvation;
    public Salvation salvationAbility;

    private Wizard wizardCharacter;

    public SoulExchange(Wizard character) : base(character, "Soul Exchange", 10) { }

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
        wizardCharacter = character as Wizard;

        attackBuff = 1.5f;
        attackRangeBuff = 1.5f;
    }

    public override void Activate()
    {
        base.Activate();
        wizardCharacter.CurrentDamage *= attackBuff;
        wizardCharacter.AttackRange *= attackRangeBuff;
    }

    public override void Passive()
    {
        if (abilityIsActivated)
        {
            timeSinceActivate += Time.deltaTime;
            if (timeSinceActivate >= wizardCharacter.timeForSalvation) // Has been in soul exchange mode for enough time
            {
                Deactivate();
                salvationAbility.abilityIsActivated = true;
            }
        }
        else if (salvationAbility.abilityIsActivated)
            salvationAbility.Passive();
    }
}
