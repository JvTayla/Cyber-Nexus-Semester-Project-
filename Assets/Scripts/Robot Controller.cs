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

    public Animator BigRobotAnim;
    public Animator LittleRobotAnim;


    public int counter=0; // Changed to int for better clarity
    public Controls playerInput; // Reference to the input actions

    private CorePowerScript _CorePowerScript;
    private HealthScript _HealthScript;
    public bool Map = false;
    public bool FinalScreen = false;
    private void Awake()
    {
        // Create a new instance of the input actions
        playerInput = new Controls();
        
        _CorePowerScript = FindObjectOfType<CorePowerScript>();
        _HealthScript = FindObjectOfType<HealthScript>();
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
           BigRobotAnim.SetBool("IsBWalking", false);
            BigRobotAnim.SetBool("IsBJumping", false);
            
            _CorePowerScript.BigRobotUI.SetActive(false);
            _CorePowerScript.SmallRobotUI.SetActive(true);
            _HealthScript.IsBigRobotInControl = false;
        
            if (!_HealthScript.IsBigRobotInControl && !FinalScreen && !Map)
            {
                _CorePowerScript.BigRobotHideDeadScreen();
                _CorePowerScript.StopBigRobotWarning();
                _CorePowerScript.BigRobotUI.SetActive(false);
            }
            else
            {
                _CorePowerScript.BigRobotUI.SetActive(false);
            }
            
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
            LittleRobotAnim.SetBool("IsWalking", false);
            LittleRobotAnim.SetBool("IsJumping", false);

            
            
            _CorePowerScript.SmallRobotUI.SetActive(false);
            _CorePowerScript.BigRobotUI.SetActive(true);
        
            _HealthScript.IsBigRobotInControl = true;

            if (_HealthScript.IsBigRobotInControl && !FinalScreen && !Map)
            {
                _CorePowerScript.SmallRobotHideDeadScreen();
                _CorePowerScript.StopSmallRobotWarning();
                _CorePowerScript.SmallRobotUI.SetActive(false);
                
            }
            else
            {
                _CorePowerScript.SmallRobotUI.SetActive(false);
            }
            
        }
        /*else if (counter == 3)
        {
            _CorePowerScript.SmallRobotUI.SetActive(false);
            _CorePowerScript.BigRobotUI.SetActive(false);
            BigRobotCam.SetActive(false);
            LittleRobotCam.SetActive(false);
        }*/
    }
}