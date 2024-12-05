using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string AbilityName;

    protected float duration;
    protected float timeSinceActivate;

    protected Ability(string abilityName, float duration, float timeSinceActivate) 
    {
        AbilityName = abilityName;
        this.duration = duration;
        this.timeSinceActivate = timeSinceActivate;
    }
    public abstract void Activate(GameObject player);
}
