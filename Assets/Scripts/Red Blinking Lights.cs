using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlinkingLights : MonoBehaviour
{
    private readonly List<Light> redLights = new List<Light>(); // Store lights that match

    private bool isBlinking = true;

    void Start()
    {
        // Get all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Loop through all GameObjects
        foreach (GameObject obj in allObjects)
        {
            // Check if the object's name is "Red Light"
            if (obj.name == "Red Lights")
            {
                // Try to get the Light component
                Light lightComponent = obj.GetComponent<Light>();

                // Check if it has a Light component and if the light color is red
                if (lightComponent != null )
                {
                    redLights.Add(lightComponent); // Add to the list of lights to blink
                }
            }
        }
        
        // If we found any red lights, start blinking
        if (redLights.Count > 0)
        {
            Debug.Log("Found");
            StartCoroutine(BlinkLights());
        }

        IEnumerator BlinkLights()
        {
            while (isBlinking)
            {
                // Turn off all red lights
                foreach (Light light in redLights)
                {
                    light.enabled = false;
                }

                // Wait for 1 second
                yield return new WaitForSeconds(1f);

                // Turn on all red lights
                foreach (Light light in redLights)
                {
                    light.enabled = true;
                }

                // Wait for another second
                yield return new WaitForSeconds(1f);
            }
        }

        // You can use this method to stop the blinking externally
        void StopBlinking()
        {
            isBlinking = false;
        }
    }
}