using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public GameObject Display;
    public GameObject Display1;
    public GameObject door;
    public GameObject[] deactivateDoor;
    public GameObject RedDisplay1;
    public GameObject RedDisplay2;
    public Lasers[] lasers; // Array of laser scripts

    void Update()
    {
        // Check if both game objects are green
        if (Display.activeSelf && Display1.activeSelf)
        {
            // Destroy the door
            Destroy(door);
            Destroy(RedDisplay1);
            Destroy(RedDisplay2);

            foreach (GameObject obj in deactivateDoor)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            // Disable all lasers
            foreach (Lasers laser in lasers)
            {
                if (laser != null)
                {
                    laser.DisableLaser();
                }
            }
        }
    }
}
