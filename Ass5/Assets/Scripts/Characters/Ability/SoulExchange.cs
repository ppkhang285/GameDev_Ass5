using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SoulExchange")]
public class SoulExchange : Ability
{
    public Salvation salvation;
    public SoulExchange() : base("Soul Exchange", 10, 10) { }

    public override void Activate(GameObject player)
    {

    }
}
