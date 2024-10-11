using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    
    public void StartGame()
    {
        anim.SetBool("StartPressed", true);
        playerInput.Player.Disable();
        SceneManager.LoadScene("CyberNexus2.0LevelDesign");
        Loginscreen.SetActive(false);
        DoorLS.SetBool("DoorOpen", true);
        DoorRS.SetBool("DoorOpen", true);

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
        }
        else
        {
            Debug.LogWarning("Info Panel not assigned in the Inspector.");
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
            Debug.LogWarning("Info Panel not assigned in the Inspector.");
        }
    }
}
