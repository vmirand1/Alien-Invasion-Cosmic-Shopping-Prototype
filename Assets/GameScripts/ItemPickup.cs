using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool isInRange = false;
    private GameObject player; // Reference to the player GameObject

    void Update()
    {
        // Check for key press and if player is in range
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Counter.Instance.IncreaseTotal();
            Destroy(gameObject); // Destroy the item
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            player = null;
        }
    }
}






