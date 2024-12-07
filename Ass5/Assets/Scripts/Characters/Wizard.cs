using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    private float manaPerCast;
    private float currentMana;
    private float hpPerCast;
    public float manaRefill;

    public float timeForSalvation;
    private Salvation salvationAbility;

    protected override void Awake()
    {
        base.Awake();

        ability = Stats.GetInstantiatedAbility() as SoulExchange;
        ability.Initialize(this);
        salvationAbility = (ability as SoulExchange).salvationAbility;

        timeForSalvation = 10;
    }

    protected override void Update()
    {
        base.Update();
        RefillMana();
    }

    protected override void HandleInput()
    {
        base.HandleInput();
        if (Input.GetKeyDown(KeyCode.F) && !salvationAbility.abilityIsActivated) // Press F to switch on/off soul exchange mode if not in salvation mode
        {
            if (ability.abilityIsActivated)
                ability.Deactivate();
            else
                ability.Activate();
        }
    }

    public override void Attack()
    {
        if (ability.abilityIsActivated) // In soul exchange mode
        {
            base.Attack();
            CurrentHP -= hpPerCast;
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

    private void RefillMana()
    {
        currentMana += manaRefill;
    }
}
