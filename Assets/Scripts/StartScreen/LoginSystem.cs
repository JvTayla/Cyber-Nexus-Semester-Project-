using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class LogInScript : MonoBehaviour
/*{
    public TMP_InputField Password;
    public GameObject[] canvas;
    public GameObject Incorrect;
    public GameObject Correct;
    public GameObject LightControllerr;

    public GameObject StartButton;
    public GameObject QuitButton;
    public GameObject OptionButton;
    public GameObject SFXButton;
    public GameObject Timer;
    public GameObject Dashboard;
    public GameObject UserIcon;
    public GameObject CorrectPassword;
    public GameObject IncorrectPassword;

    private void Start()
    {
        canvas[0].SetActive(true);
    }

    public void success()
    {
        string pass = Password.text;

        if (pass == "CYBERNEXUS")
        {
            Debug.Log("open");
            Incorrect.SetActive(false);
            Correct.SetActive(true);
            Alarm alarm = LightControllerr.GetComponent<Alarm>();

            if (alarm != null)
            {
                alarm.StopAlarm(); // Call method to stop the alarm coroutine and disable the script
            }
            alarm.enabled = false;


            //Turn On Stuff
            StartButton.SetActive(true);
            QuitButton.SetActive(true);
            OptionButton.SetActive(true);
            SFXButton.SetActive(true);
            Dashboard.SetActive(true);

            //Move Things
            Timer.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            Timer.transform.position = new Vector3(394, 209, 0);


            //Turn Off Stuff
            UserIcon.SetActive(false);
            CorrectPassword.SetActive(false);
            IncorrectPassword.SetActive(false);

        }

        else
        {
            Debug.Log("error");
            Incorrect.SetActive(true);
            Correct.SetActive(false);
        }
    }


}*/

{
    // Public variables to assign in the Inspector
    public TMP_InputField Password;
    public GameObject Incorrect;
    public GameObject Correct;
    public GameObject LightControllerr;

    public GameObject StartButton;
    public GameObject QuitButton;
    public GameObject OptionButton;
    public GameObject SFXButton;
    public GameObject Timer;
    public GameObject Timer1;
    public GameObject Dashboard;
    public GameObject UserIcon;
    public GameObject CorrectPassword;
    public GameObject IncorrectPassword;
    public GameObject InputScreen;

    // Coroutine to handle the success logic
    public IEnumerator Success()
    {
        string pass = Password.text;
        Debug.Log("Password entered: " + pass); // Log entered password

        if (pass == "CYBERNEXUS") // Check if the password is correct
        {
            Debug.Log("Password correct");
            Incorrect.SetActive(false);
            Correct.SetActive(true);
            Alarm alarm = LightControllerr.GetComponent<Alarm>();

            if (alarm != null)
            {
                alarm.StopAlarm(); // Call method to stop the alarm coroutine and disable the script
            }
            alarm.enabled = false;

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            // Turn On Stuff
            /*StartButton.SetActive(true);
            QuitButton.SetActive(true);
            OptionButton.SetActive(true);
            SFXButton.SetActive(true);
            Dashboard.SetActive(true);*/

            // Move Things + Scale
            SwapPositions(Timer, Timer1);

            Timer.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);

            // Turn Off Stuff
            UserIcon.SetActive(false);
            CorrectPassword.SetActive(false);
            IncorrectPassword.SetActive(false);
            InputScreen.SetActive(false);
        }
        else
        {
            Debug.Log("Password incorrect");
            Incorrect.SetActive(true);
            Correct.SetActive(false);
        }
    }


    private void SwapPositions(GameObject obj1, GameObject obj2)
    {
        Vector3 tempPosition = obj1.transform.position; // Store position of obj1
        obj1.transform.position = obj2.transform.position; // Swap
        obj2.transform.position = tempPosition; 

        Debug.Log($"Swapped positions: {obj1.name} at {obj1.transform.position}, {obj2.name} at {obj2.transform.position}");
    }


    // Method to call the Success coroutine when a button is clicked
    public void OnLoginButtonClicked()
    {
        StartCoroutine(Success());
    }


}


