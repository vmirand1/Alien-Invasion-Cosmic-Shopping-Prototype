using UnityEngine;
using Cinemachine;

public class FreeLookCameraControl : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    private CinemachineFreeLook.Orbit[] originalOrbits;
    private float originalXAxisValue;
    private float originalYAxisValue;

    void Start()
    {
        // Save the original orbits to restore later
        originalOrbits = new CinemachineFreeLook.Orbit[freeLookCamera.m_Orbits.Length];
        for (int i = 0; i < originalOrbits.Length; i++)
        {
            originalOrbits[i] = freeLookCamera.m_Orbits[i];
        }

        // Save the original axis values
        originalXAxisValue = freeLookCamera.m_XAxis.Value;
        originalYAxisValue = freeLookCamera.m_YAxis.Value;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Enable camera control when right mouse button is pressed
            freeLookCamera.m_XAxis.m_MaxSpeed = 300;
            freeLookCamera.m_YAxis.m_MaxSpeed = 3;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            // Disable camera control when right mouse button is released
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }
    }
}

