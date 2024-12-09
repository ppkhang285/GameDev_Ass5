using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character
{
    public int ammoPerRound;
    public int currentAmmo;
    private float reloadTime;
    private float timeSinceReload;

    public int targetHits;
    private int hitsForPierceShot; // number of hits needed to activate pierce shot 

    protected override void Awake()
    {
        base.Awake();

        ability = Stats.GetInstantiatedAbility() as PierceShot;
        ability.Initialize(this);

        ammoPerRound = 10;
        currentAmmo = ammoPerRound;
        reloadTime = 5;
        timeSinceReload = 0;
        targetHits = 0;
        hitsForPierceShot = 5;
    }
    
    protected override void Update()
    {
        base.Update();
        if ((targetHits >= hitsForPierceShot) & !ability.abilityIsActivated)
            ability.Activate();
        ReloadAmmo();
        GameplayManager.Instance.hudManager.UpdateSpecialHUD(currentAmmo, ammoPerRound);
    }

    public override void Attack()
    {
        base.Attack();
        if (!ability.abilityIsActivated)
        {
            if (currentAmmo > 0)
                currentAmmo--;
            else
                return;
        }
        ArrowManager.Instance.SpawnArrow(this);
    }

    public void HitTarget()
    {
        if (!ability.abilityIsActivated)
            targetHits++;
    }

    private void ReloadAmmo()
    {
        if (currentAmmo == 0) // No ammo left
        {
            if (timeSinceReload < Time.deltaTime)
                animator.SetTrigger("reload");
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
