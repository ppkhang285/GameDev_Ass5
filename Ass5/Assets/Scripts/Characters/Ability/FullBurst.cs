using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FullBurst")]
public class FullBurst : Ability
{
    private float attackBuff;
    private float attackCooldownReduction;
    private float speedBuff;
    private float resistenceDebuff;

    private Berserker berserkerCharacter; 

    public FullBurst(Berserker character) : base(character, "Full Burst", 10) { }

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        berserkerCharacter = character as Berserker;

        attackBuff = 1.5f;
        attackCooldownReduction = 2.4f;
        speedBuff = 1.3f;
        resistenceDebuff = 1.2f;
    }

    public override void Activate()
    {
        base.Activate();
        berserkerCharacter.CurrentDamage *= attackBuff;
        berserkerCharacter.AttackCooldown /= attackCooldownReduction;
        berserkerCharacter.Speed *= speedBuff;
        berserkerCharacter.Resistence /= resistenceDebuff;
        berserkerCharacter.animator.SetTrigger("spin");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        berserkerCharacter.Rage = 0;
        berserkerCharacter.animator.SetTrigger("spin");
        berserkerCharacter.TakeDamage(0);
    }





}
