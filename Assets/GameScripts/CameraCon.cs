using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public Transform camTarget; // Reference to the player's transform
    public float distance = 5f; // Distance from the player
    public float height = 2f;   // Height above the player
    public float smoothSpeed = 0.125f; // Smooth transition speed

    void LateUpdate()
    {
        // Calculate the desired position behind the player
        Vector3 desiredPosition = camTarget.position - camTarget.forward * distance + Vector3.up * height;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the player's position
        transform.LookAt(camTarget.position + Vector3.up * height / 2);
    }
}


