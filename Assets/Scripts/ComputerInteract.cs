using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    public FirstPersonControls FirstPersonControls;
    public int numberOfConsol;
    public GameObject ConsolOn;
    public GameObject ConsolOff;

    private void Update()
    {
     if (FirstPersonControls.consultCount == numberOfConsol) 
        {
            ConsolOff.SetActive(false);
            ConsolOn.SetActive(true);
        }
    }
}
