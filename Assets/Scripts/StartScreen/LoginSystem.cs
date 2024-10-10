using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogInScript : MonoBehaviour
{
    
    public TMP_InputField Password;
    //public Text openCode;

    public GameObject[] canvas;
    public GameObject Incorrect;
    public GameObject Correct;
    public GameObject LightControllerr;

    public GameObject StartButton;
    public GameObject QuitButton;
    public GameObject OptionButton;


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

            StartButton.SetActive(true);
            QuitButton.SetActive(true);
            OptionButton.SetActive(true);

        }

        else
        {
            Debug.Log("error");
            Incorrect.SetActive(true);
            Correct.SetActive(false);
        }
    }

}


