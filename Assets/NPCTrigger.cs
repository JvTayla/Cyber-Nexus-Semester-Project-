using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
  
    NPCAnimator npcanimator;
    private UIScript _UIScript;
    private BigRobotController _BigRobotController;
    private int i = 0;
    
    private void Start()
    { 
        npcanimator = FindObjectOfType<NPCAnimator>(); // This finds the first NPCAnimator in the scene
        _UIScript = FindObjectOfType<UIScript>();
        _BigRobotController = FindAnyObjectByType<BigRobotController>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot")) //Mightneedtochangeif we have Diff Tags for Robots
        {
            
            npcanimator.StartInteraction();
            _UIScript.InteractWithNpc();
            
            if (i == 0)
            {
                _UIScript.MissionTasks();
                i++;
            }


           // _BigRobotController.playerInput.Player.Disable();
        }

    }
}
