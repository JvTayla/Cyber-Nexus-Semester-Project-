using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    public GameObject BigRobot;
    public GameObject SmallRobot;
    
    private float RobotChargingDistance = 10;

    public bool IsBigRobotInControl = true;
    
    //Big Robot
    public TextMeshProUGUI BtimerText;  // Reference to TextMeshProUGUI component
    private int BcurrentValue = 100;    // Starting value
    private float BnextUpdate = 0f;     // Time for the next update

    public GameObject BChargingImg;
    public GameObject BChargerImg;
    //Small Robot
    public TextMeshProUGUI StimerText;  // Reference to TextMeshProUGUI component
    private int ScurrentValue = 100;    // Starting value
    private float SnextUpdate = 0f;     // Time for the next update
    public GameObject SChargingImg;

    public GameObject SChargerImg;

    private CorePowerScript _CorePowerScript;

    public GameObject MissionFailed;

    private SoundScript _SoundScript;

    private FirstPersonControls _FirstPersonControls;

    private RobotController _RobotController;

    private BigRobotController _BigRobotController;
    
    // Start is called before the first frame update
    void Start()
    {
        _CorePowerScript = FindObjectOfType<CorePowerScript>();
        _FirstPersonControls = FindObjectOfType<FirstPersonControls>();
        _RobotController = FindObjectOfType<RobotController>();
        _BigRobotController = FindAnyObjectByType<BigRobotController>();
    }

    // Update is called once per frame
    void Update()
    {
        RobotsHealthPower();
    }
    
    private void decBigRobotTimer()
    {        
        // Check if it's time to update the text (every second)
        if (Time.time >= BnextUpdate)     // Time for the next update)
        {
            BnextUpdate = Time.time + 1; // Set next update time
            DecreasePowerBigRobot(IsBigRobotInControl);
        }
    }
    private void incBigRobotTimer()
    {        
        // Check if it's time to update the text (every second)
        if (Time.time >= BnextUpdate)     // Time for the next update)
        {
            BnextUpdate = Time.time + 1; // Set next update time
            IncreasePowerBigRobot();
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void DecreasePowerBigRobot(bool BigRobotInControl)
    {
        
        if (!BigRobotInControl)
        {
            _CorePowerScript.StopBigRobotWarning();
            _CorePowerScript.BigRobotHideDeadScreen();
            _CorePowerScript.BigRobotUI.SetActive(false);
        }
        else
        {
            if (!(BcurrentValue > 0)) // Ensure the value doesn't go below 0
            {
                _CorePowerScript.StopBigRobotWarning();
                _CorePowerScript.BigRobotShowDeadScreen();
                _CorePowerScript.BigRobotDead = true;
             
            }
            else if (BcurrentValue is > 0 and < 10 )
            {
                _CorePowerScript.BigRobotDead = false;
                _CorePowerScript.BigRobotCoreWarning();
                BcurrentValue--; // Decrease the number by 1
                BtimerText.text = BcurrentValue.ToString() + "%"; // Update the displayed text with percentage
               
            }
            else
            {
                _CorePowerScript.BigRobotDead = false;
                _CorePowerScript.StopBigRobotWarning();
                _CorePowerScript.BigRobotHideDeadScreen();
                BcurrentValue--; // Decrease the number by 1
                BtimerText.text = BcurrentValue.ToString() + "%"; // Update the displayed text with percentage
               
            }
            
            if (_SoundScript != null && _SoundScript.warningSound != null)
            {
                if (!(BcurrentValue > 0) && !_SoundScript.warningSound.activeSelf) // Ensure the value doesn't go below 0
                {
                    _SoundScript.StopWarningSound();
                }
                else if (BcurrentValue is > 0 and < 10 && _SoundScript.warningSound.activeSelf)
                {
                    _SoundScript.PlayWarningSound();
                }
                else if (!_SoundScript.warningSound.activeSelf)
                {
                    _SoundScript.StopWarningSound();
                }
            }
           
        }
    }
    
    private void IncreasePowerBigRobot()
    {
        if (BcurrentValue < 100) // Ensure the value doesn't go over 100
        {
            BcurrentValue++; // Increase the number by 1
            BtimerText.text = BcurrentValue.ToString() + "%"; // Update the displayed text with percentage
        }
    }
    
    private void decSmallRobotTimer()
    {        
        // Check if it's time to update the text (every second)
        if (Time.time >= SnextUpdate)     // Time for the next update)
        {
            SnextUpdate = Time.time + 1; // Set next update time
            DecreasePowerSmallRobot(false);
        }
    }
    
    private void incSmallRobotTimer()
    {        
        // Check if it's time to update the text (every second)
        if (Time.time >= SnextUpdate)     // Time for the next update)
        {
            SnextUpdate = Time.time + 1; // Set next update time
            IncreasePowerSmallRobot();
        }
    }
    
    private void DecreasePowerSmallRobot(bool BigRobotInControl)
    {
        if (BigRobotInControl)
        {
            _CorePowerScript.StopSmallRobotWarning();
            _CorePowerScript.SmallRobotHideDeadScreen();
            _CorePowerScript.SmallRobotUI.SetActive(false);
            
        }
        else
        {
            if (!(ScurrentValue > 0)) // Ensure the value doesn't go below 0
            {
                _CorePowerScript.StopSmallRobotWarning();
                _CorePowerScript.SmallRobotShowDeadScreen();
                _CorePowerScript.SmallRobotDead = true;
                
            }
            else if (ScurrentValue is > 0 and < 10)
            {
                _CorePowerScript.SmallRobotCoreWarning();
                ScurrentValue--; // Decrease the number by 1
                StimerText.text = ScurrentValue.ToString() + "%";
                _CorePowerScript.SmallRobotDead = false;// Update the displayed text with percentage
             
            }
            else
            {
                _CorePowerScript.SmallRobotDead = false;
                _CorePowerScript.StopSmallRobotWarning();
                _CorePowerScript.SmallRobotHideDeadScreen();
                ScurrentValue--; // Decrease the number by 1
                StimerText.text = ScurrentValue.ToString() + "%"; // Update the displayed text with percentage
               
            }
            
            if (_SoundScript != null && _SoundScript.warningSound != null)
            {
                if (!(BcurrentValue > 0) && !_SoundScript.warningSound.activeSelf) // Ensure the value doesn't go below 0
                {
                    _SoundScript.StopWarningSound();
                }
                else if (BcurrentValue is > 0 and < 10 && !_SoundScript.warningSound.activeSelf)
                {
                    _SoundScript.PlayWarningSound();
                }
                else if (!_SoundScript.warningSound.activeSelf)
                {
                    _SoundScript.StopWarningSound();
                }
            }
        }
        
    }
    
    private void IncreasePowerSmallRobot()
    {
        if (ScurrentValue < 100 ) // Ensure the value doesn't go over 100
        {
            ScurrentValue++; // Increase the number by 1
            StimerText.text = ScurrentValue.ToString() + "%"; // Update the displayed text with percentage
        }
    }

    private void RobotsHealthPower()
    {
        if (!AreRobotsCharging())
        {
            if (IsBigRobotInControl)
            {
                decBigRobotTimer();
            }
            else
            {
                decSmallRobotTimer();
            }
        }
        else
        {
            incBigRobotTimer();
            incSmallRobotTimer();
        }

        if (_CorePowerScript.SmallRobotDead && _CorePowerScript.BigRobotDead)
        {
            MissionFailed.SetActive(true);
            _FirstPersonControls.playerInput.Player.Disable();
            _RobotController.playerInput.Player.Disable();
            //_SoundScript.PlayLoseMusic();
        }
    }

    private bool AreRobotsCharging()
    {
        if (CalculateDistance(BigRobot.transform.position, SmallRobot.transform.position) < 100)
        {
            BChargingImg.SetActive(true);
            SChargingImg.SetActive(true);
            BChargerImg.SetActive(false);
            SChargerImg.SetActive(false);
            return true;
        }
        
        BChargingImg.SetActive(false);
        SChargingImg.SetActive(false);
        BChargerImg.SetActive(true);
        SChargerImg.SetActive(true);
        
        return false;

    }
    
    // Function to calculate distance between two points in 3D space
    public float CalculateDistance(Vector3 X, Vector3 Y)
    {
        // Calculate and return the distance using Vector3.Distance
        return Vector3.Distance(X, Y);
    }

    public void ResetGame()
    {
        _FirstPersonControls.playerInput.Player.Disable();
        _BigRobotController.playerInput.Player.Disable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }


    public void ReturnToMainMenu()
    {
        // Load the main menu scene (ensure you have the correct scene name in your build settings)
        SceneManager.LoadScene("StartCyberNexus");
    }

    // Function to exit the game
    public void ExitGame()
    {
        // If running in the Unity editor, exit play mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running a build, exit the application
            Application.Quit();
        #endif
    }
}
