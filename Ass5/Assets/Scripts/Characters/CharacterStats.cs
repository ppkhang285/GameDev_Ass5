using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterStats", menuName = "CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public float hp;
    public float damage;
    public float attackRange;
    public Sprite sprite;
}