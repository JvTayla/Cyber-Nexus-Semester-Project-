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
            
        }

        else
        {
            Debug.Log("error");
            Incorrect.SetActive(true);
            Correct.SetActive(false);
        }
    }

}