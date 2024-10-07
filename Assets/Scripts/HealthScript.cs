using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    // Start is called before the first frame update
    void Start()
    {
        
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
            DecreasePowerBigRobot();
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
    
    private void DecreasePowerBigRobot()
    {
        if (BcurrentValue > 0) // Ensure the value doesn't go below 0
        {
            BcurrentValue--; // Decrease the number by 1
            BtimerText.text = BcurrentValue.ToString() + "%"; // Update the displayed text with percentage
        }
    }
    
    private void IncreasePowerBigRobot()
    {
        if (BcurrentValue < 100) // Ensure the value doesn't go over 000
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
            DecreasePowerSmallRobot();
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
    
    private void DecreasePowerSmallRobot()
    {
        if (ScurrentValue > 0) // Ensure the value doesn't go below 0
        {
            ScurrentValue--; // Decrease the number by 1
            StimerText.text = ScurrentValue.ToString() + "%"; // Update the displayed text with percentage
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

    public void RobotsHealthPower()
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
}
