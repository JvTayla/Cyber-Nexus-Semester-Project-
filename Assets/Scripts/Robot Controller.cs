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
    

    public float counter;

    public bool isBabyRobot;

  
    private void Update()
    {
        RoboSwitch();
    }

    public void RoboSwitch()
    {
      

        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            counter++;
            

            //print("llalala");
            if (counter == 1) 
            {
                Debug.Log("switch to small boi");
                BigRobotCam.SetActive(false);
                LittleRobotCam.SetActive(true);
                BigRobotController.enabled = false;
                FirstPersonControls.enabled = true;
                AudioListenerLil.enabled = true;
                AudioListenerBig.enabled = false;
                

                
               
            }

                if (counter == 2) 
            {
                Debug.Log("switch to big boi");
                BigRobotCam.SetActive(true);
                LittleRobotCam.SetActive(false);
                BigRobotController.enabled = true;
                FirstPersonControls.enabled = false;
                AudioListenerLil.enabled = false;
                AudioListenerBig.enabled = true;
                counter = 0;
            }
            
        }
    }

}
