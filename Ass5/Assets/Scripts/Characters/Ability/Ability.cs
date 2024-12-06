using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string AbilityName;

    protected float duration;
    protected float timeSinceActivate;
    protected bool hitTarget;

    protected Ability(string abilityName, float duration, float timeSinceActivate) 
    {
        AbilityName = abilityName;
        this.duration = duration;
        this.timeSinceActivate = timeSinceActivate;
    }

    public virtual bool CheckActivateCondition()
    {
        return true;
    }

    public virtual void Activate(GameObject player)
    {
        return;
    }

    public virtual void Deactivate(GameObject player)
    {
        Character character = player.GetComponent<Character>();
        character.ResetStats(); // Return player to normal state
    }

    public virtual void Passive(GameObject player)
    {
        hitTarget = false;
        if (CheckActivateCondition())
            timeSinceActivate = 0;
        if (timeSinceActivate < duration) // Ability still has effect
        {
            if (timeSinceActivate <= 0) // First call when ability is activated
                Activate(player);
            timeSinceActivate += Time.deltaTime;
        }
        else
            Deactivate(player);
    }

    public virtual void Attack(GameObject player) // Default attack logic
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        Character character = player.GetComponent<Character>();
        character.Attack();
        playerController.timeSinceLastAttack = 0;
        // TODO: add logic to check if hit target
    }
}
