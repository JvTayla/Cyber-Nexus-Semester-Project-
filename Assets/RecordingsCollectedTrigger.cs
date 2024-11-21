using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingsCollectedTrigger : MonoBehaviour
{

    public Animator holoanimator;
    public Animator bodyanimator;

    NPCAnimator npcanimator;
    private UIScript _UIScript;
    private BigRobotController _BigRobotController;
    private NPCTalking _NPCTalking;
    public GameObject NPCYapCam;


    private void Start()
    {
        npcanimator = FindObjectOfType<NPCAnimator>(); // This finds the first NPCAnimator in the scene
        _BigRobotController = FindAnyObjectByType<BigRobotController>();
        _NPCTalking = FindAnyObjectByType<NPCTalking>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot")) //Mightneedtochangeif we have Diff Tags for Robots
        {
            if (_BigRobotController.Allrecordings)
            {
                NPCYapCam.SetActive(true);
                StartCoroutine(_NPCTalking.ThirdYap());
                Yap();
            }
        }
    }

    public IEnumerator Yap()
    {
        holoanimator.SetBool("IsTalking", true);// Let Himtalk for however long the text is
        bodyanimator.SetBool("IsTalking", true);

        yield return new WaitForSeconds(14f); //Need to Time Properly for text

        NPCYapCam.SetActive(false);

        holoanimator.SetBool("IsTalking", false);
        bodyanimator.SetBool("IsTalking", false);
        holoanimator.SetBool("IsIdle", true);// StartTheIdleAnimations
        bodyanimator.SetBool("IsIdle", true);


    }

    public IEnumerator StopYap()
    {

        holoanimator.SetBool("IsTalking", false);
        bodyanimator.SetBool("IsTalking", false);
        holoanimator.SetBool("IsIdle", true);// StartTheIdleAnimations
        bodyanimator.SetBool("IsIdle", true);
        Debug.Log("FinishedYap2");
        yield return new WaitForSeconds(2f);
        NPCYapCam.SetActive(false);

    }
}

