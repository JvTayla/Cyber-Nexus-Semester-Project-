using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour

{
    public Light[] Lights; // Array of Light components
    public Renderer[] WallLightsMaterial; // Array of Renderers for materials
    public GameObject[] WallLights; // Array of wall light GameObjects
    public Material Red; // Material for red light
    public Material Blue; // Material for blue light

    private float AlarmTime = 2f; // Time for alarm state
    private float AlarmEmission = 8000f; // Intensity during alarm
    private Coroutine alarmCoroutine; // Store reference to coroutine

    private void Start()
    {
        alarmCoroutine = StartCoroutine(ActivateAlarm()); // Start and store coroutine reference
    }

    private IEnumerator ActivateAlarm()
    {
        while (true) // Loop indefinitely
        {
            foreach (GameObject wall in WallLights)
            {
                wall.SetActive(true); // Correctly activate each wall light
            }

            foreach (Light light in Lights)
            {
                light.enabled = true;
                light.intensity = AlarmEmission;
            }

            foreach (Renderer renderer in WallLightsMaterial)
            {
                if (renderer.materials.Length > 1)
                {
                    Material[] materialsCopy = renderer.materials;
                    materialsCopy[1] = Red;
                    renderer.materials = materialsCopy;
                }
            }

            yield return new WaitForSeconds(AlarmTime);

            foreach (Light light in Lights)
            {
                light.enabled = false;
            }

            foreach (Renderer renderer in WallLightsMaterial)
            {
                if (renderer.materials.Length > 1)
                {
                    Material[] materialsCopy = renderer.materials;
                    materialsCopy[1] = Blue;
                    renderer.materials = materialsCopy;
                }
            }

            foreach (GameObject wall in WallLights)
            {
                wall.SetActive(false);
            }

            yield return new WaitForSeconds(3f);
        }
    }

    public void StopAlarm() // Method to stop the alarm coroutine
    {
        if (alarmCoroutine != null)
        {
            foreach (Light light in Lights)
            {
                light.enabled = false;
            }

            foreach (Renderer renderer in WallLightsMaterial)
            {
                if (renderer.materials.Length > 1)
                {
                    Material[] materialsCopy = renderer.materials;
                    materialsCopy[1] = Blue;
                    renderer.materials = materialsCopy;
                }
            }

            foreach (GameObject wall in WallLights)
            {
                wall.SetActive(false);
            }

            StopCoroutine(alarmCoroutine);


            alarmCoroutine = null; // Clear the reference after stopping


        }
    }
}