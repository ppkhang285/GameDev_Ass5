using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string AbilityName;

    public float duration;
    protected float timeSinceActivate;
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
        timeSinceActivate = duration;
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
        if (abilityIsActivated)
        {
            if (timeSinceActivate < duration)
                timeSinceActivate += Time.deltaTime;
            else
                Deactivate();
        }
    }
}
