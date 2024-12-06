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
    private int hitsForPierceShot = 5; // number of hits needed for pierce shot 

    private float attackBuff = 1.5f;
    private float speedBuff = 1.2f;

    private bool isPiercing = false;

    public PierceShot() : base("Pierce Shot", 10, 10) { }

    public override bool CheckActivateCondition()
    {
        return targetHits >= hitsForPierceShot;
    }

    public override void Activate(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        character.CurrentDamage *= attackBuff;
        character.MovementSpeed *= speedBuff;
    }

    public override void Deactivate(GameObject player)
    {
        base.Deactivate(player);
        currentAmmo = ammoPerRound; // Refill all ammo
    }

    public override void Passive(GameObject player)
    {
        base.Passive(player);

        // Other passive mechanics of the character
        ReloadAmmo();
    }

    public override void Attack(GameObject player)
    {
        if (isPiercing) // In pierce shot mode
            base.Attack(player);
        else
        {
            if (currentAmmo > 0) // Still has ammo
            {
                base.Attack(player);
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
