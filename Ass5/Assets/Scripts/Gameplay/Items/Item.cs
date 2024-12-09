using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void OnPickup(GameObject player)
    {
        Destroy(gameObject); 
    }
}
