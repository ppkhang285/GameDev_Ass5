using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FullBurst")]
public class FullBurst : Ability
{
    private float attackBuff;
    private float attackSpeedBuff;
    private float speedBuff;
    private float resistenceDebuff;

    private Berserker berserkerCharacter; 

    public FullBurst(Berserker character) : base(character, "Full Burst", 10) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        berserkerCharacter = character as Berserker;

        attackBuff = 1.5f;
        attackSpeedBuff = 1.2f;
        speedBuff = 1.2f;
        resistenceDebuff = 0.9f;
    }

    public override void Activate()
    {
        base.Activate();
        berserkerCharacter.CurrentDamage *= attackBuff;
        berserkerCharacter.AttackSpeed *= attackSpeedBuff;
        berserkerCharacter.MovementSpeed *= speedBuff;
        berserkerCharacter.Resistence *= resistenceDebuff;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        berserkerCharacter.Rage = 0;
    }

    

    

}
