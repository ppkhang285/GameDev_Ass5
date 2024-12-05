using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FullBurst")]
public class FullBurst : Ability
{
    private float maxRage = 10;
    public float Rage { get; set; }

    private float attackBuff = 1.5f;
    private float attackSpeedBuff = 1.2f;
    private float speedBuff = 1.2f;
    private float resistenceDebuff = 0.9f;

    public FullBurst() : base("Full Burst", 10, 10) { }

    public override void Activate(GameObject player)
    {
        if (Rage >= maxRage)
            timeSinceActivate = 0;

        Character character = player.GetComponent<Character>();
        if (timeSinceActivate < duration) // Ability still has effect
        {
            if (timeSinceActivate <= 0) // First call when ability is activated
            {
                Rage = 0;
                character.CurrentDamage *= attackBuff;
                character.AttackSpeed *= attackSpeedBuff;
                character.MovementSpeed *= speedBuff;
                character.Resistence *= resistenceDebuff;
            }
            timeSinceActivate += Time.deltaTime;
        }
        else
        {
            // Return player to normal state
            character.ResetStats();
        }

        // Other passive mechanics of the character
        FillRage();
    }

    private void FillRage()
    {

    }
}
