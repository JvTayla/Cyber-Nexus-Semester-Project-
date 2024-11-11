using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Unity.Burst.CompilerServices;

public class Finallogin : MonoBehaviour

{
    // Public variables to assign in the Inspector
    public TMP_InputField Password;
    public GameObject Incorrect;
    public GameObject Correct;
    public GameObject BigRobotCam;
    public GameObject BigRobotUI;
    public GameObject LittleRobotUI;
    public GameObject LittleRobotCam;

    /*public GameObject StartButton;
    public GameObject QuitButton;
    public GameObject OptionButton;
    //public GameObject SFXButton;*/
    public GameObject Timer;
  
    //public GameObject Dashboard;
    
    public GameObject CorrectPassword;
    public GameObject IncorrectPassword;
    
    public GameObject Hint;
    public GameObject startFinalScene;   // GameObject for the "Start Final Scene"



    FinalAnimationController FinalAnimationController;

    // Coroutine to handle the success logic
    


    public void OnLoginButtonClicked()
    {
        BigRobotCam.SetActive(false);
        BigRobotUI.SetActive(false);
        LittleRobotUI.SetActive(false);
        LittleRobotCam.SetActive(false);
        StartCoroutine(Success());

    }

    public IEnumerator Success()
    {
        string pass = Password.text;
        Debug.Log("Password entered: " + pass); // Log entered password

        if (pass == "CYBERNEXUS") // Check if the password is correct
        {
            Debug.Log("Password correct");
            Incorrect.SetActive(false);
            Correct.SetActive(true);
        

            // Wait for 5 seconds
            yield return new WaitForSeconds(1.5f);

            // Turn On Stuff
            /*StartButton.SetActive(true);
            QuitButton.SetActive(true);
            OptionButton.SetActive(true);
            /*SFXButton.SetActive(true);*/
    
            startFinalScene.SetActive(true);

            

            // Turn Off Stuff
            
            CorrectPassword.SetActive(false);
            IncorrectPassword.SetActive(false);
            Hint.SetActive(false);
            Timer.SetActive(false);
        }
        else
        {
            Debug.Log("Password incorrect");
            Incorrect.SetActive(true);
            Correct.SetActive(false);
        }
    }
}
