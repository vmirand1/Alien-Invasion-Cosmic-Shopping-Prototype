using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotator : MonoBehaviour
{ 
    public Vector2 turn;
    public float sensitivity = 0.5f; //sensitivity of camera movement
    public Transform character;  // Reference to the character

    void Start()
    {
        // Ensure the pivot starts at the same position as the character
        transform.position = character.position;
        Cursor.lockState = CursorLockMode.Locked; // cursor invisible
    }

    void Update()
    {
        // Rotate based on mouse input
        if (Input.GetMouseButton(1))
        {
            turn.y += Input.GetAxis("Mouse Y") * sensitivity;
            turn.x += Input.GetAxis("Mouse X") * sensitivity;
        }

        // Clamp the vertical rotation to prevent flipping
        turn.y = Mathf.Clamp(turn.y, -30, 60);

        // Apply rotation around the character
        transform.position = character.position;
        transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
