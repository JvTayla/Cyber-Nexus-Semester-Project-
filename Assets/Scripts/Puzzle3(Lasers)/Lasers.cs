using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject SwitchDisplay, SwitchDisplay1, SwitchOn, SwitchOff;
    public Respawn respawn;
    public float laserDistance = 500f; // New configurable distance for both raycast and line renderer

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        lineRenderer.SetPosition(0, transform.position); // Set starting point of the laser
        RaycastHit hit;

        // Cast the ray with the maximum range laserDistance
        if (Physics.Raycast(transform.position, transform.forward, out hit, laserDistance))
        {
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, hit.point); // Laser ends at the hit point

                // Check if the laser hits a player
                if (hit.collider.CompareTag("Robot"))
                {
                    SwitchDisplay.SetActive(true);
                    SwitchDisplay1.SetActive(false);
                    SwitchOff.SetActive(true);
                    SwitchOn.SetActive(false);
                    respawn.RespawnPlayer();
                }
            }
        }
        else
        {
            // If nothing is hit, extend the laser to the maximum distance
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
        }
    }
}
