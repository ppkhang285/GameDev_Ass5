using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : Character
{
    private float maxRage;
    private float rage;
    public float Rage
    {
        get { return rage; }
        set
        {
            if (value < 0) rage = 0;
            else if (value > maxRage) rage = maxRage;
            else rage = value;
        }
    }

    private float rageIncreasePerHit;
    private float rageDecreaseSpeed;

    protected override void Awake()
    {
        base.Awake();

        ability = Stats.GetInstantiatedAbility() as FullBurst;
        ability.Initialize(this);

        maxRage = 6;
        Rage = 0;
        rageIncreasePerHit = 0.25f;
        rageDecreaseSpeed = 2 * Time.deltaTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if ((rage >= maxRage) & !ability.abilityIsActivated)
            ability.Activate();
        DecreaseRage();
        BuffAttack();
    }

    public void HitTarget()
    {
        Rage += rageIncreasePerHit;
    }

    private void DecreaseRage() // Rage decreases when not attacking
    {
        Rage -= rageDecreaseSpeed;
    }

    private void BuffAttack()
    {
        if (!ability.abilityIsActivated)
            CurrentDamage = Stats.damage * (1 + 0.25f * rage / maxRage);
    }

}
