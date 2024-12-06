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

    private bool isSoulExchange = false;
    private float timeForSalvation = 10;

    private float attackBuff = 1.5f;
    private float attackRangeBuff = 1.5f;

    public Salvation salvation;
    private Salvation salvationAbility;

    public SoulExchange() : base("Soul Exchange", 10) { }

    private void OnEnable()
    {
        if (salvation != null)
        {
            salvationAbility = Instantiate(salvation);
            salvationAbility.Initialize(this);
        }
        else
            salvationAbility = null;
    }

    public override void Activate(GameObject player)
    {
        isSoulExchange = true;
        timeSinceActivate = 0;
        Character character = player.GetComponent<Character>();
        character.CurrentDamage *= attackBuff;
        character.AttackRange *= attackRangeBuff;
        // Debug.Log("Ability " + AbilityName + " lasts for " + duration + " seconds");
    }

    public override void Deactivate(GameObject player)
    {
        base.Deactivate(player);
        isSoulExchange = false;
    }

    public override void Passive(GameObject player)
    {
        if (Input.GetKeyDown(KeyCode.F) && !salvationAbility.isSalvation) // Press F to switch on/off soul exchange mode if not in salvation mode
        {
            if (isSoulExchange)
                Deactivate(player);
            else
                Activate(player);
        }

        // Other passive mechanics of the character
        RefillMana();
        if (isSoulExchange)
            InSoulExchange(player);
        else if (salvationAbility.isSalvation)
            salvationAbility.Passive(player);
    }

    public override void Attack(GameObject player)
    {
        if (isSoulExchange) // In soul exchange mode
        {
            base.Attack(player);
            player.GetComponent<Character>().CurrentHP -= hpPerCast;
        }
        else
        {
            if (currentMana >= manaPerCast) // Enough mana to cast
            {
                base.Attack(player);
                currentMana -= manaPerCast;
            }
        }
    }

    private void InSoulExchange(GameObject player)
    {
        timeSinceActivate += Time.deltaTime;
        if (timeSinceActivate >= timeForSalvation) // Has been in soul exchange mode for enough time
        {
            Deactivate(player);
            salvationAbility.isSalvation = true;
        }
    }

    private void RefillMana()
    {
        currentMana += manaRefill;
    }
}
