
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixElectrical : MonoBehaviour
{
    private BigRobotController _BigRobotController;
    private FirstPersonControls _FirstPersonControls;
    private PuzzleScript _PuzzleScript;
    public GameObject DoorTrigger;
    public GameObject PowerNeededBB;
    public GameObject PowerNeededSB;


    private void Start()
    {
       
        _FirstPersonControls = FindAnyObjectByType<FirstPersonControls>();
        _BigRobotController = FindAnyObjectByType<BigRobotController>();
        _PuzzleScript = FindAnyObjectByType<PuzzleScript>();
        //  npcanimator = FindObjectOfType<NPCAnimator>(); // This finds the first NPCAnimator in the scene
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot")) //Mightneedtochangeif we have Diff Tags for Robots
        {

            
            
                StartCoroutine(NeedPower());
                //ShowTextForDuration(5.0f);
                Debug.Log("You need a Security Tag to open this door.");

            


        }

    }

    private IEnumerator NeedPower()
    {
        Debug.Log("Door not opened!");
        PowerNeededBB.SetActive(true);
        PowerNeededSB.SetActive(true);
        yield return new WaitForSeconds(5f);

        PowerNeededBB.SetActive(false);
        PowerNeededSB.SetActive(false);
    }
}

