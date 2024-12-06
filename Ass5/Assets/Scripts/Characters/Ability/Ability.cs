using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string AbilityName;

    protected float duration;
    protected float timeSinceActivate;
    protected bool hitTarget;
    public bool abilityIsActivated;

    protected Character character;

    protected Ability(Character character, string abilityName, float duration) 
    {
        this.character = character;
        AbilityName = abilityName;
        this.duration = duration;
        timeSinceActivate = duration;
    }

    public virtual void Initialize(Character character)
    {
        this.character = character;
        abilityIsActivated = false;
    }

    public virtual bool CheckActivateCondition()
    {
        return true;
    }

    public virtual void Activate()
    {
        abilityIsActivated = true;
        timeSinceActivate = 0;
    }

    public virtual void Deactivate()
    {
        abilityIsActivated = false;
        character.ResetStats(); // Return player to normal state
    }

    public virtual void Passive()
    {
        hitTarget = false;
        if (CheckActivateCondition())
            Activate();
        if (timeSinceActivate < duration) // Ability still has effect
            timeSinceActivate += Time.deltaTime;
        else
            Deactivate();
    }

    // Some abilities may affect player's move mechanics
    public virtual void Move()
    {
        return;
    }

    // Some abilities may affect player's attack mechanics
    public virtual void Attack() 
    {
        character.animator.SetTrigger("attack");
        character.TimeSinceLastAttack = 0;
        // TODO: add logic to check if hit target
    }

    // Some abilities may affect player's take damage mechanics
    public virtual void TakeDamage(float damage) 
    {
        return;
    }
}
