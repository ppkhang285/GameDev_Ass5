using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PierceShot")]
public class PierceShot : Ability
{
    private float attackBuff;
    private float speedBuff;

    private Archer archerCharacter;

    public PierceShot(Archer character) : base(character, "Pierce Shot", 10) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        archerCharacter = character as Archer;

        attackBuff = 1.5f;
        speedBuff = 1.2f;
    }

    public override void Activate()
    {
        base.Activate();
        archerCharacter.CurrentDamage *= attackBuff;
        archerCharacter.Speed *= speedBuff;
        archerCharacter.targetHits = 0;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        archerCharacter.currentAmmo = archerCharacter.ammoPerRound; // Refill all ammo
    }
}
