using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlinkingLights : MonoBehaviour
{
    private readonly List<Light> redLights = new List<Light>(); // Store lights that match

    private bool isBlinking = true;
    public Color lightBulbColor = new Color(1.0f, 0.95f, 0.8f);  // Light bulb color (warm white)
    private Coroutine blinkingCoroutine;  // Store the reference to the coroutine

    void Start()
    {
        // Get all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Loop through all GameObjects
        foreach (GameObject obj in allObjects)
        {
            // Check if the object's name is "Red Lights"
            if (obj.name == "Red Lights")
            {
                // Try to get the Light component
                Light lightComponent = obj.GetComponent<Light>();

                // Check if it has a Light component
                if (lightComponent != null)
                {
                    redLights.Add(lightComponent); // Add to the list of lights to blink
                }
            }
        }

        // If we found any red lights, start blinking
        if (redLights.Count > 0)
        {
            Debug.Log("Found");
            blinkingCoroutine = StartCoroutine(BlinkLights());
        }
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

    // Method to stop blinking and hide the lights
    public void StopBlinking()
    {
        // Stop the blinking
        isBlinking = false;

        // Stop the coroutine to prevent further blinking
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
        }

        // Hide the lights by disabling their GameObjects
        foreach (Light light in redLights)
        {
            light.gameObject.SetActive(false);  // Hide the GameObjects
        }

        Debug.Log("Blinking stopped, lights are hidden.");
    }
}
