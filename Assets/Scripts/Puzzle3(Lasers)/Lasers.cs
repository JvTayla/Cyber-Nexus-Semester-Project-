using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject SwitchDisplay, SwitchDisplay1, SwitchOn, SwitchOff;
    public Respawn respawn;
    public float laserDistance = 500f; // Maximum laser distance
    private bool isLaserActive = true; // Laser state

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (!isLaserActive)
        {
            lineRenderer.enabled = false; // Turn off the laser visuals
            return;
        }

        lineRenderer.enabled = true; // Ensure the laser visuals are on
        lineRenderer.SetPosition(0, transform.position); // Set starting point of the laser

        RaycastHit hit;

        // Cast the ray with the maximum range laserDistance
        if (Physics.Raycast(transform.position, transform.forward, out hit, laserDistance))
        {
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, hit.point); // Laser ends at the hit point

                if (hit.collider.CompareTag("Robot"))
                {
                    SwitchDisplay.SetActive(true);
                    SwitchDisplay1.SetActive(false);
                    SwitchOff.SetActive(true);
                    SwitchOn.SetActive(false);

                    string robotName = hit.collider.gameObject.name;

                    if (robotName == "Big Robot")
                    {
                        respawn.RespawnPlayer("Big Robot");
                    }
                    else if (robotName == "Baby Robot ")
                    {
                        respawn.RespawnPlayer("Baby Robot ");
                    }
                    else
                    {
                        Debug.LogWarning("Unknown robot hit: " + robotName);
                    }
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
        }
    }

    // Method to disable the laser
    public void DisableLaser()
    {
        isLaserActive = false;
    }
}
