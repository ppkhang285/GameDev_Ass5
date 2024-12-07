using UnityEngine;

public class ArrowCollision : MonoBehaviour
{
    public Arrow associatedArrow; 

    void OnTriggerEnter(Collider other)
    {
        if (associatedArrow != null && associatedArrow.IsActive)
        {
            if (other.CompareTag("Character"))
            {
                associatedArrow.IsActive = false; 
                HandleCollision(other.gameObject); 
            }
        }
    }

    private void HandleCollision(GameObject character)
    {
        associatedArrow.archer.HitTarget();
        gameObject.SetActive(false);
    }
}
