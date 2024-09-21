using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIgRobotHeadBobbingHead : MonoBehaviour
{
    public GameObject Camera;
   

    public void StartBobbing()
    {

        Camera.GetComponent<Animator>().Play("HeadBobbingBigRobot");

    }

    public void StopBobbing()
    {
        Camera.GetComponent<Animator>().Play("New State");
    }
}
