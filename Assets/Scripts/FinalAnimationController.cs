using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAnimationController : MonoBehaviour

{
    public GameObject startFinalScene;   // GameObject for the "Start Final Scene"
    public GameObject middleScene;       // GameObject for the "Middle Scene"
    public GameObject finalScene;        // GameObject for the "Final Scene"

    // Function for the first button (OpenFile)
    public void OpenFile()
    {
        middleScene.SetActive(true); // Activate the "Start Final Scene" GameObject
        Debug.Log("Middle Scene activated");
    }

    // Function for the second button (UploadData)
    public void UploadData()
    {
        finalScene.SetActive(true); // Activate the "Middle Scene" GameObject
        Debug.Log("Final activated");
    }

    // Function for the third button (ContinueToEnd)
    public void ContinueToEnd()
    {
        //Need to make end screen UI and Buttons
        //finalScene.SetActive(true); // Activate the "Final Scene" GameObject
        Debug.Log("Final Scene activated");
    }
}

