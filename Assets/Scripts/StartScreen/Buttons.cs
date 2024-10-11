using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Buttons : MonoBehaviour
{
    private Controls playerInput;
    public GameObject ControlPanel;
    public GameObject Loginscreen;
    public Animator anim;
    public Animator DoorRS;
    public Animator DoorLS;



    private void Start()
    {
        playerInput = new Controls();
        playerInput.Player.Enable();

    }
    
    public IEnumerator StartGame()
    {
        print("StartButtonPressed");

        anim.SetBool("StartPressed", true);
        playerInput.Player.Disable();
        
        Loginscreen.SetActive(false);


        yield return new WaitForSeconds(0.5F);

        DoorLS.SetBool("DoorOpen", true);
        DoorRS.SetBool("DoorOpen", true);

        yield return new WaitForSeconds(3.5F);

        SceneManager.LoadScene("CyberNexus2.0LevelDesign");

    }
    

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Controls()
    {
        if (ControlPanel != null)
        {
            ControlPanel.SetActive(true);
            print("StartButtonPressed");
        }
        else
        {
            print("Info Panel not assigned in the Inspector.");
        }
    }

    public void Back()
    {
        if (ControlPanel != null)
        {
            ControlPanel.SetActive(false); // Deactivate the information panel
        }
        else
        {
            print("Info Panel not assigned in the Inspector.");
        }
    }

    public void StartGaming()
    {
        StartCoroutine(StartGame());
    }
}
