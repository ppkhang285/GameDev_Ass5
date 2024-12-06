using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PierceShot")]
public class PierceShot : Ability
{
    private int ammoPerRound = 10;
    private int currentAmmo = 10;
    private float reloadTime = 3;
    private float timeSinceReload = 3;

    private int targetHits = 0;
    private int hitsForPierceShot = 5; // number of hits needed to activate pierce shot 

    private float attackBuff = 1.5f;
    private float speedBuff = 1.2f;

    public PierceShot(Character character) : base(character, "Pierce Shot", 10) { }

    public override bool CheckActivateCondition()
    {
        return (targetHits >= hitsForPierceShot) & !abilityIsActivated;
    }

    public override void Activate()
    {
        base.Activate();
        character.CurrentDamage *= attackBuff;
        character.MovementSpeed *= speedBuff;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        currentAmmo = ammoPerRound; // Refill all ammo
    }

    public override void Passive()
    {
        base.Passive();

        // Other passive mechanics of the character
        ReloadAmmo();
    }

    public override void Attack()
    {
        if (abilityIsActivated) // In pierce shot mode
            base.Attack();
        else
        {
            if (currentAmmo > 0) // Still has ammo
            {
                base.Attack();
                currentAmmo--;
                if (hitTarget)
                    targetHits++;
            }
        }
    }

    private void ReloadAmmo()
    {
        if (currentAmmo == 0) // No ammo left
        {
            if (timeSinceReload >= reloadTime)
            {
                timeSinceReload = 0;
                currentAmmo = ammoPerRound;
            }
            else
                timeSinceReload += Time.deltaTime;
        }
    }
}
