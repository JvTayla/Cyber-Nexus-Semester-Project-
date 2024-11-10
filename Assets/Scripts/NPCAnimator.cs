using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    public Transform player;  // The player's transform
    public float detectionRange = 20f;  // Range within which the NPC detects the player
    public float rotationSpeed = 5f;  // Speed at which the NPC rotates to face the player
    public GameObject NPCCam;
    public GameObject NPCTrigger;

    public bool IsTalking =false;
    public bool IsBasePowered =false ;
    public bool IsIdle = false;
    public Animator holoanimator;
    public Animator bodyanimator;
    public Animator Camanimator;
    public GameObject[] OpenDoor; 

    // Start is called before the first frame update
    
    public void Start()
    {

        IsBasePowered = true; //This is after MK Puzzle is completed essentially 
    }

    public void StartInteraction()
    {
        if (IsBasePowered == true)
        {
            NPCCam.SetActive(true);
            Camanimator.SetBool("PuzzlSolved", true);
            holoanimator.SetBool("BasePoweredOn", true);
            bodyanimator.SetBool("BasePoweredOn", true);
            StartCoroutine(Talking());
        }
    }

    private void Update() //FollowAurora
    {
        // Calculate the distance between the NPC and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // If the player is within the detection range
        if (distance <= detectionRange)
        {
            // Make the NPC rotate to face the player
            Vector3 direction = player.position - transform.position;
            direction.y = 0;  // Keep the NPC's rotation on the Y axis only
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public IEnumerator Talking()
    {
        yield return new WaitForSeconds(16f);
        IsBasePowered = false;
        IsTalking = true;

        if (IsTalking == true)
        {
           
            holoanimator.SetBool("BasePoweredOn", false);//Stop the Animation for the sequence and go back to default state
            bodyanimator.SetBool("BasePoweredOn", false);
            holoanimator.SetBool("IsTalking", true);// Let Himtalk for however long the text is
            bodyanimator.SetBool("IsTalking", true);
            StartCoroutine(Idle());


        }
        else //IdleAnimations should start here 
        {
            StartCoroutine(Idle());
            holoanimator.SetBool("BasePoweredOn", false);//Stop the Animation for the sequence and go back to default state
            bodyanimator.SetBool("BasePoweredOn", false);
            holoanimator.SetBool("IsTalking", false);
            bodyanimator.SetBool("IsTalking", false);
            holoanimator.SetBool("IsIdle", true);// StartTheIdleAnimations
            bodyanimator.SetBool("IsIdle", true);
        }


    }

    public  IEnumerator Idle() //Use this when he stops talking  (Just add in IsIdle=true; and Istalking=false into code for text or say StartCouroutine(Idle()); )
    {
        yield return new WaitForSeconds(10f);
        IsBasePowered = false;
        IsTalking = false;
        IsIdle = true;

        foreach (GameObject door in OpenDoor)
        {
            if (door != null)
            {
                door.SetActive(false);
            }
        }

        if (IsTalking==false && IsIdle==true)
        {
            holoanimator.SetBool("BasePoweredOn", false);//Stop the Animation for the sequence and go back to default state
            bodyanimator.SetBool("BasePoweredOn", false);
            holoanimator.SetBool("IsTalking", false);
            bodyanimator.SetBool("IsTalking", false);
            holoanimator.SetBool("IsIdle", true);// StartTheIdleAnimations
            bodyanimator.SetBool("IsIdle", true);

            StartCoroutine(StopYapping());
        }
     
    }

    public IEnumerator StopYapping()
    {
        Debug.Log("FinishedYapping");
        yield return new WaitForSeconds(2f);
        NPCCam.SetActive(false);
        Destroy(NPCTrigger);
    }
}
