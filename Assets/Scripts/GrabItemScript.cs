using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItemScript : MonoBehaviour
{
    private FirstPersonControls firstPersonControls; // Reference to the First Person Control

    private float x, y, z;
    // Start is called before the first frame update
    void Start()
    {
        firstPersonControls = FindObjectOfType<FirstPersonControls>(); //Storing all public functions and variables in firstpersonControls from the FirstPersonControls script
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /* private void OnTriggerEnter(Collider other)
        {
            if (firstPersonControls.laserBeam.activeSelf && other.gameObject.CompareTag("Clue")) // checks if the game object is active and is in contact with the glue
            {
                // the moment the laser hits the clue it gets the current position of the laser beam and stores it in its respective coordinate in the 3D space
                x = firstPersonControls.laserBeam.transform.position.x; // current x position
                y = firstPersonControls.laserBeam.transform.position.y; // current y position
                z = firstPersonControls.laserBeam.transform.position.z; // current z position
                
                Debug.Log("Collide");
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (firstPersonControls.laserBeam.activeSelf && other.gameObject.CompareTag("Clue"))// checks if the game object is active and is in contact with the glue
            {
                Vector3 transformPosition = other.gameObject.transform.position; // stores the clues vector 3 Coordinates in a variable called transform position
                
                transformPosition.x += firstPersonControls.laserBeam.transform.position.x - x ; 
                transformPosition.y += firstPersonControls.laserBeam.transform.position.y - y ;
                transformPosition.z += firstPersonControls.laserBeam.transform.position.z - z ;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!firstPersonControls.laserBeam.activeSelf && other.gameObject.CompareTag("Clue")) // checks if the game object is not active
            {
                
             
            }
           
        }*/
}
