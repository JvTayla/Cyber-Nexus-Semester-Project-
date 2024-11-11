using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecurityClearancedoor : MonoBehaviour
{
    /*public GameObject bigRobot;
    public GameObject smallRobot;*/
    private SecurityClearancedoor _SecurityClearacedoor;
    private BigRobotController _BigRobotController;
    private FirstPersonControls _FirstPersonControls;
    public GameObject Trigger;

    public GameObject[] SecurityDoorOpened; //GameObject to switch off (RedDoor)
    public GameObject[] SecurityDoorOpening; //GameObject to set Active (Green Door)
    public Text Text2takeout; 
    //public Animator DoorAnimator;


    private void Start()
    {   
        _SecurityClearacedoor = FindAnyObjectByType<SecurityClearancedoor>();
        _FirstPersonControls = FindAnyObjectByType<FirstPersonControls>();
        _BigRobotController = FindAnyObjectByType<BigRobotController>();
        //  npcanimator = FindObjectOfType<NPCAnimator>(); // This finds the first NPCAnimator in the scene
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot")) //Mightneedtochangeif we have Diff Tags for Robots
        {
            
            if ( /*_FirstPersonControls.HasSecurityTag == true ||*/ _BigRobotController.HasSecurityTag == true)
            {
                Debug.Log("AccessGranted");
                StartCoroutine(OpenDoor());
                
            }
            else
            {
                //ShowTextForDuration(5.0f);
                Debug.Log("You need a Security Tag to open this door.");
            }

            /*BigRobotController controller = bigRobot.GetComponent<BigRobotController>();
            if (controller!=null)
            {
                AttemptToOpenDoor(controller.HasSecurityTag);
            }
            FirstPersonControls controls = smallRobot.GetComponent<FirstPersonControls>();
            if(controls!= null)
            {
                AttemptToOpenDoor(controls.HasSecurityTag);
            }*/

        }
        
    }

    /*private void AttemptToOpenDoor(bool HasSecurityTag)
    {
        if (HasSecurityTag == true)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("You need a Security Tag to open this door.");
        }
    }*/
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
        Destroy(Trigger);
    }
    /*public IEnumerator ShowTextForDuration(float duration)
    {
        Text2takeout.text = "Take Out Your SecurityTag.";
        Text2takeout.gameObject.SetActive(true); // Show the text
        yield return new WaitForSeconds(duration); // Wait for 5 seconds (or the given duration)
        Text2takeout.gameObject.SetActive(false); // Hide the text after 5 seconds
    }*/
}

