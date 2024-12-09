using UnityEngine;

public class HealthItem : Item
{
    public int healthAmount = 10; 

    public override void OnPickup(Character player)
    {
        base.OnPickup(player);
        player.CurrentHP += healthAmount;
    }
}
