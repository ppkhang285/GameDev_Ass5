using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    private float maxMana;
    private float manaPerCast;

    private float mana;
    private float CurrentMana
    {
        get { return mana; }
        set
        {
            if (value < 0) mana = 0;
            else if (value > maxMana) mana = maxMana;
            else mana = value;
        }
    }
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

        maxMana = 100;
        manaPerCast = 15;
        CurrentMana = 0;
        hpPerCast = 1;
        manaRefill = 0.2f;
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
            if (CurrentMana >= manaPerCast) // Enough mana to cast
            {
                base.Attack();
                CurrentMana -= manaPerCast;
            }
        }
    }

    private void RefillMana()
    {
        CurrentMana += manaRefill;
    }
}
