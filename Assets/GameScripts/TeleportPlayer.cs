using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportPlayer : MonoBehaviour
{
    public Transform player;
    public Transform destination;
    public GameObject playerg;
    void OnTriggerEnter(Collider other) { // When player collides with sender object
        if(other.CompareTag("Player")) { 
            playerg.SetActive(false); // Player object set to false to ensure teleports
            playerg.transform.position = destination.position; // Player sent to destination
            playerg.SetActive(true);
        }
    }
}

