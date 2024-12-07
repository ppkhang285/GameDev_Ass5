using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/GuardPoint")]
public class GuardPoint : Ability
{
    private float attackBuff;

    private Knight knightCharacter;

    public GuardPoint(Knight character) : base(character, "Guard Point", 5) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        knightCharacter = character as Knight;

        attackBuff = 1.2f;
    }

    public override void Activate()
    {
        base.Activate();
        knightCharacter.timeSinceBlock = knightCharacter.timeForGuardPoint;
        knightCharacter.CurrentDamage *= attackBuff;
        knightCharacter.Resistence = 1;
    }
}