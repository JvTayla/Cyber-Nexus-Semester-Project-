using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public GameObject Display;
    public GameObject Display1;
    public GameObject door;

    // Update is called once per frame
    void Update()
    {
        // Check if both game objects are green
        if (Display.activeSelf && Display1.activeSelf)
        {
            // Destroy the door
            Destroy(door);
        }
        
    }

    // Helper method to check if a game object is green
   




}
