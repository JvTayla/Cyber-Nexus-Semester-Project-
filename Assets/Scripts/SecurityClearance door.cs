using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityClearancedoor : MonoBehaviour
{
    public GameObject bigRobot;
    public GameObject smallRobot;

    public GameObject[] SecurityDoorOpened; //GameObject to switch off (RedDoor)
    public GameObject[] SecurityDoorOpening; //GameObject to set Active (Green Door)

    //public Animator DoorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot")) //Mightneedtochangeif we have Diff Tags for Robots
        {
            BigRobotController controller = bigRobot.GetComponent<BigRobotController>();
            if (controller!=null)
            {
                AttemptToOpenDoor(controller.HasSecurityTag);
            }
            FirstPersonControls controls = smallRobot.GetComponent<FirstPersonControls>();
            if(controls!= null)
            {
                AttemptToOpenDoor(controls.HasSecurityTag);
            }
           
        }
        
    }

    private void AttemptToOpenDoor(bool HasSecurityTag)
    {
        if (HasSecurityTag == true)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("You need a Security Tag to open this door.");
        }
    }
    private IEnumerator OpenDoor()
    {
        Debug.Log("Door opened!");
        //DoorAnimator.SetBool("DoorOpening", true);

        yield return new WaitForSeconds(5f);

        foreach (GameObject obj in SecurityDoorOpened)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        foreach (GameObject obj in SecurityDoorOpening)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        Debug.Log("Door opened!");
    }

}

