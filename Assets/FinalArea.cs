using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class FinalArea : MonoBehaviour
{
    public GameObject LoginScreen;
    public GameObject FinalCamara;
    public GameObject FinalScreen;
    public GameObject LittleRobotUI;
    public GameObject LittleRobotCam;
    public GameObject BigRobotUI;
    public GameObject BigRobotCam;
    public GameObject BatteryDeadCanvas;

    public GameObject[] Display;

    public ConditionalVideoPlaylist videoPlaylist;  // Reference to the ConditionalVideoPlaylist component

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot")) // Might need to change if we have different tags for robots
        {
            StartCoroutine(FinalScene());
        }
    }

    public IEnumerator FinalScene()
    {
        yield return new WaitForSeconds(1f);

        LoginScreen.SetActive(true);
        FinalCamara.SetActive(true);

        BatteryDeadCanvas.SetActive(false);
        BigRobotUI.SetActive(false);
        BigRobotCam.SetActive(false);
        LittleRobotCam.SetActive(false);
        LittleRobotUI.SetActive(false);

        yield return new WaitForSeconds(5f);

        foreach (GameObject obj in Display)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        // Start the video playback sequence
        FinalScreen.SetActive(true);
        if (videoPlaylist != null)
        {
            Debug.Log("Attempting to play animation");
            videoPlaylist.PlayVideos();
        }
        else
        {
            Debug.LogWarning("videoPlaylist reference is missing in FinalArea.");
        }

        
    }
}
