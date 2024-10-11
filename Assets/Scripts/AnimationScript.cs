using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Allows using UnityEvent for custom functions

public class AnimationScript : MonoBehaviour
{
    public Animator animator1;  // Animator for the first animation
    public Animator animator2;  // Animator for the second animation

    public string animation1Name;  // Name of the first animation
    public string animation2Name;  // Name of the second animation

    public Camera originalCamera;   // Reference to the original camera
    public Camera targetCamera;      // Reference to the camera to switch to

    private bool isAnimationPlaying = false; // To check if an animation is currently playing

    private PuzzleScript _PuzzleScript;

    private void Start()
    {
        _PuzzleScript = FindObjectOfType<PuzzleScript>();
    }
    // Function to play both animations in sequence and switch cameras
    public void PlayBothAnimations()
    {
        if (!isAnimationPlaying)
        {
            StartCoroutine(PlayAnimationsAndSwitchCameras());
        }
        else
        {
            Debug.LogWarning("An animation is already playing.");
        }
    }

    // Coroutine to play both animations and switch cameras
    private IEnumerator PlayAnimationsAndSwitchCameras()
    {
        isAnimationPlaying = true; // Set flag to indicate animations are playing

        // Switch to the target camera
        SwitchCamera(targetCamera);

        // Play the first animation
        animator1.Play(animation1Name);
        yield return new WaitForSeconds(animator1.GetCurrentAnimatorStateInfo(0).length); // Wait until the first animation is done
        OnAnimation1Complete(); // Call the callback function for Animation 1

        // Play the second animation
        animator2.Play(animation2Name);
        yield return new WaitForSeconds(animator2.GetCurrentAnimatorStateInfo(0).length); // Wait until the second animation is done
        OnAnimation2Complete(); // Call the callback function for Animation 2

        // Switch back to the original camera
        SwitchCamera(originalCamera);

        isAnimationPlaying = false; // Reset flag
    }

    // Function to switch cameras
    private void SwitchCamera(Camera cameraToActivate)
    {
        originalCamera.gameObject.SetActive(cameraToActivate == originalCamera);
        targetCamera.gameObject.SetActive(cameraToActivate == targetCamera);
    }

    // Callback function for when Animation 1 finishes
    private void OnAnimation1Complete()
    {
        Debug.Log("Animation 1 finished.");
        CustomFunctionAfterAnimation1();
    }

    // Callback function for when Animation 2 finishes
    private void OnAnimation2Complete()
    {
        Debug.Log("Animation 2 finished.");
        CustomFunctionAfterAnimation2();
    }

    // Custom function to execute after Animation 1
    private void CustomFunctionAfterAnimation1()
    {
        // Add your code here
        _PuzzleScript.DoorOpener();
    }

    // Custom function to execute after Animation 2
    private void CustomFunctionAfterAnimation2()
    {
        // Add your code here
        _PuzzleScript.DoorOpener();
    }
}


