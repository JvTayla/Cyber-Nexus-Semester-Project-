using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Unity.VisualScripting;


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

        
        // Start is called before the first frame update
    private void Start()
    {
        playerInput = new Controls();
        playerInput.Player.Enable();
        playerInput.Player.Startgame.performed += ctx => StartGame();

    }

    // Update is called once per frame
    void StartGame()
    {
        anim.SetBool("isPressed", true);
        
        playerInput.Player.Disable();
        playerInput.Player.Startgame.performed -= ctx => StartGame();  
        Startscreen.SetActive(false);
        StartCoroutine (Wait());



    }

    
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);

      
        LoginScreen.SetActive(true);
        LoginScreenCam.SetActive(true);
    }

}
