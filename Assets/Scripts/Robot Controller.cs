using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public GameObject BigRobot;
    public GameObject BigRobotCam;
    public GameObject LittleRobot;
    public GameObject LittleRobotCam;

    public BigRobotController BigRobotController;
    public FirstPersonControls FirstPersonControls;
    public AudioListener AudioListenerLil;
    public AudioListener AudioListenerBig;

    private int counter; // Changed to int for better clarity
    private Controls playerInput; // Reference to the input actions

    private void Awake()
    {
        // Create a new instance of the input actions
        playerInput = new Controls();
    }

    private void OnEnable()
    {
        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the SwitchRobot action
        playerInput.Player.SwitchRobot.performed += ctx => RoboSwitch();
    }

    private void OnDisable()
    {
        // Disable the input actions and unsubscribe from the action
        playerInput.Player.SwitchRobot.performed -= ctx => RoboSwitch();
        playerInput.Player.Disable();
    }

    public void RoboSwitch()
    {
        counter++;

        if (counter == 1)
        {
            Debug.Log("Switch to small robot");
            BigRobotCam.SetActive(false);
            LittleRobotCam.SetActive(true);
            BigRobotController.enabled = false;
            FirstPersonControls.enabled = true;
            AudioListenerLil.enabled = true;
            AudioListenerBig.enabled = false;
        }
        else if (counter == 2)
        {
            Debug.Log("Switch to big robot");
            BigRobotCam.SetActive(true);
            LittleRobotCam.SetActive(false);
            BigRobotController.enabled = true;
            FirstPersonControls.enabled = false;
            AudioListenerLil.enabled = false;
            AudioListenerBig.enabled = true;
            counter = 0; // Reset the counter after switching
        }
    }
}