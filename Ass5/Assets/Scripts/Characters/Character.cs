using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CharacterStats characterStats;

    public CharacterStats stats
    {
        get { return characterStats; }
    }

    public float CurrentHP { get; set; }
    public float CurrentDamage { get; set; }

    void Start()
    {
        CurrentHP = characterStats.hp;
        CurrentDamage = characterStats.damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
