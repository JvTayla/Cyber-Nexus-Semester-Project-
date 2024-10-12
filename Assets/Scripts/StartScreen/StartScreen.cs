using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Unity.VisualScripting;
using System;


public class StartScreen : MonoBehaviour
{
    private Controls playerInput;
    public GameObject Startscreen;
    public GameObject LoginScreen;
    public GameObject LoginScreenCam;
    public GameObject StartButton;
    public GameObject QuitButton;
    public GameObject OptionButton; 
    public Animator anim;
    public Animator DoorRS;
    public Animator DoorLS;

    public GameObject FirstScreen;



    public Renderer ConsoleMaterial;

    public GameObject ConsoleKey;

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = new Controls();
        playerInput.Player.Enable();
        playerInput.Player.Startgame.performed += ctx => StartGame();
        StartCoroutine(Loadstartscreen());

    }

    // Update is called once per frame
    void StartGame()
    {
        anim.SetBool("isPressed", true);

        playerInput.Player.Disable();
        playerInput.Player.Startgame.performed -= ctx => StartGame();
        Startscreen.SetActive(false);
        StartCoroutine(Wait());
      //StartCoroutine(L());


    }

    private IEnumerator Loadstartscreen()
        {
        yield return new WaitForSeconds(5f);

        FirstScreen.SetActive(false);
        Startscreen.SetActive(true);



    }


    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);


        LoginScreen.SetActive(true);
        LoginScreenCam.SetActive(true);
        TurnOffAnimator();
        SetEmissionIntensity(1.5f);
    }

    private void SetEmissionIntensity(float intensity)
    {
        if (ConsoleMaterial != null)
        {
            Material mat = ConsoleMaterial.material; // Get the material instance
            Color emissionColor = mat.GetColor("_EmissionColor");

            // Set the emission color with the specified intensity
            mat.SetColor("_EmissionColor", emissionColor * intensity);

            // Update the global illumination for the material
            DynamicGI.SetEmissive(ConsoleMaterial, emissionColor * intensity); // Use ConsoleMaterial as Renderer
        }
    }

    public void TurnOffAnimator()
    {
        if (ConsoleKey != null)
        {
            Animator consoleKeyAnimator = ConsoleKey.GetComponent<Animator>();

            if (consoleKeyAnimator != null)
            {
                consoleKeyAnimator.enabled = false; // Disable the Animator
            }
        }
    }
}
