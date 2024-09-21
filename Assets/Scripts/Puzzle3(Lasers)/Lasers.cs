using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lasers : MonoBehaviour
{
    private LineRenderer lineRenderer; 
    public GameObject SwitchDisplay,SwitchDisplay1, SwitchOn, SwitchOff;
    public Respawn respawn; 
   

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, hit.point);

                // Check if the laser hits a player
                if (hit.collider.CompareTag("Player"))
                {
                    SwitchDisplay.SetActive(true); 
                    SwitchDisplay1.SetActive(false); 
                    SwitchOff.SetActive(true); 
                    SwitchOn.SetActive(false);
                    respawn.RespawnPlayer();
                    
                }
            }
            else
            {
                lineRenderer.SetPosition(1, transform.forward * 5000);
            }
        }
    }  
    
} 















