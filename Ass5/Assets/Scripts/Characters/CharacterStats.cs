using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    // Player
    Knight,
    Berserker,
    Archer,
    Wizard,
    
    // Enemy
    Warrior,
    Rogue,
    Mage, 
    Minion
}

[CreateAssetMenu(fileName = "New CharacterStats", menuName = "CharacterStats")]
public class CharacterStats : ScriptableObject
{
    // Base stats
    public float hp; 
    public float damage; 
    public float attackRange; // Range for missile character, for melee character, it will be the size of the weapon
    public float speed; // Movement speed
    public float jumpHeight; 
    public float attackCooldown; // Seconds before the character can attack again
    public float resistence; // Percentage of damage reduced, 0 = receive full damage, 1 = ignore all damage, negative number = receive more damage
    public CharacterType charType;
    public Ability ability;

    public Ability GetInstantiatedAbility()
    {
        if (ability != null)
        {
            return Instantiate(ability);
        }
        return null; 
    }
}