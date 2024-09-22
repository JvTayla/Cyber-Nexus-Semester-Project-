using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SmallRobotHeadBobbing : MonoBehaviour
{
    public GameObject Camera;
   

   public void StartBobbing()
    {

        Camera.GetComponent<Animator>().Play("HeadBobbingSmallRobot");

    }

    public void StopBobbing()
    {
        Camera.GetComponent<Animator>().Play("New State");
    }
}
