using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string AbilityName;
    public float Cooldown;

    protected Ability(string abilityName) 
    {
        AbilityName = abilityName;
    }
    public abstract void Activate(); 
}
