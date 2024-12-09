using UnityEngine;

public class HealthItem : Item
{
    public int healthAmount = 10; 

    public override void OnPickup(GameObject player)
    {
        base.OnPickup(player); 
        Character character = player.GetComponent<Character>();
        character.CurrentHP += healthAmount;
    }
}
