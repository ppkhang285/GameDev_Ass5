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
    public float hp;
    public float damage;
    public float attackRange;
    public float attackSpeed;
    public float movementSpeed;
    public float jumpHeight;
    public float attackCooldown;
    public float resistence;
    public CharacterType charType;
    public Ability ability;
}