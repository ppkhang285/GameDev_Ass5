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

        maxRage = 10;
        Rage = 0;
        rageIncreasePerHit = 0.5f;
        rageDecreaseSpeed = 1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if ((rage >= maxRage) & !ability.abilityIsActivated)
            ability.Activate();
        DecreaseRage();
        BuffAttack();
        GameplayManager.Instance.hudManager.UpdateSpecialHUD(Rage, maxRage);
    }

    public override void Move(float horizontal, float vertical)
    {
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (!ability.abilityIsActivated)
            animator.SetFloat("speed", direction.magnitude);
        transform.Translate(direction * Speed * Time.deltaTime, Space.Self);
    }

    public override void Attack()
    {
        TimeSinceLastAttack = 0;
        if (!ability.abilityIsActivated) 
            animator.SetTrigger("attack");
    }

    public void HitTarget()
    {
        Rage += rageIncreasePerHit;
    }

    private void DecreaseRage() // Rage decreases when not attacking
    {
        Rage -= rageDecreaseSpeed * Time.deltaTime;
    }

    private void BuffAttack()
    {
        if (!ability.abilityIsActivated)
            CurrentDamage = Stats.damage * (1 + rage / maxRage);
    }

}
