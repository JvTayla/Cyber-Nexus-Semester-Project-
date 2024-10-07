using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // sounds
    // ui 
    /* //makes a raycase from the baby camera where the mouse is pointing
            Ray ray = new Ray(_FirstPersonControls.playerCamera.position, _FirstPersonControls.playerCamera.forward);
            RaycastHit hit;
            
            // Debugging: Draw the ray in the Scene view
            Debug.DrawRay(_FirstPersonControls.playerCamera.position, _FirstPersonControls.playerCamera.forward * _FirstPersonControls.pickUpRange, Color.red, 2f);

            
            if (Physics.Raycast(ray, out hit, _FirstPersonControls.pickUpRange*2))
            {
                //checks if the hit of the raycast has a tag of the specific tags shown below
                if (hit.collider.CompareTag("OtherTiles") || hit.collider.CompareTag("StraightTiles"))
                {
                    //store the hit of the raycast in the tile gameobject
                    GameObject tile = hit.collider.gameObject;
                    
                    //rotate the tile by 90 degrees
                    var angles = tile.transform.eulerAngles;
                    angles.z -= 90f;
                    tile.transform.eulerAngles = angles;
                }
            } */
}
