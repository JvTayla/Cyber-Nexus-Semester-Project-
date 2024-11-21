using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionGame : MonoBehaviour
{

    public GameObject Introscreen;
    // Start is called before the first frame update
    void Start()
    {
        Introscreen.SetActive(true);
        StartCoroutine(IntroScreen());
     
        Debug.Log("MissionDebriefstarted");
    }

    // Update is called once per frame
    private IEnumerator IntroScreen()
    {
        yield return new WaitForSeconds(30f);
        Debug.Log("GameStarted");
        Introscreen.SetActive(false);
    }
}
