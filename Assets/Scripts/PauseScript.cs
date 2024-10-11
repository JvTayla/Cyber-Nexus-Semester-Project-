using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    private Controls playerInput;
    public Button[] pauseButtons;
    private int currentButtonIndex = 0;
    private Vector3[] originalScales;
    private const float movementThreshold = 0.8f;
    public GameObject controlPage;
    public GameObject pausePage; 

   

    private void Awake()
    {
        playerInput = new Controls();
        playerInput.PauseMenu.Enable();
        playerInput.Player.Disable();

        originalScales = new Vector3[pauseButtons.Length];
        for (int i = 0; i < pauseButtons.Length; i++)
        {
            originalScales[i] = pauseButtons[i].transform.localScale;
        }
    }

    public void ControlsScreen()
    {
        Debug.Log("controls pressed");
        controlPage.SetActive(true);
        pausePage.SetActive(false);
    }

    public void Update()
    {
        Vector2 moveInput = playerInput.PauseMenu.Movement.ReadValue<Vector2>();

        if (moveInput.y > 0 || moveInput.y < 0)
        {
            NavigateMenu(moveInput);

        }

        if (playerInput.PauseMenu.Select.triggered)
        {
            SelectButton();
        }
        if (playerInput.PauseMenu.Back.triggered)
        {
            BackButton();
        }
    }

    public void BackButton()
    {
        Debug.Log("Back pressed");
        controlPage.SetActive(false);
        pausePage.SetActive(true);
    }

    public void NavigateMenu(Vector2 moveInput)
    {
        if (Mathf.Abs(moveInput.y) > movementThreshold)
        {
            if (moveInput.y > 0)
            {
                currentButtonIndex--;

                if (currentButtonIndex < 0)
                {
                    currentButtonIndex = pauseButtons.Length - 1;
                }
                Debug.Log("Moved up, current button index: " + currentButtonIndex);
            }

            else if (moveInput.y < 0)
            {
                currentButtonIndex++;

                if (currentButtonIndex >= pauseButtons.Length)
                    currentButtonIndex = 0;

            }
            UpdateButtonSelection();
        }
    }

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < pauseButtons.Length; i++)
        {
            if (i == currentButtonIndex)
            {
                // Highlight the selected button by scaling it up
                pauseButtons[i].transform.localScale = originalScales[i] * 1.2f;
            }
            else
            {
                // Reset the scale for non-selected buttons
                pauseButtons[i].transform.localScale = originalScales[i];
            }
        }
    }

    public void SelectButton()
    {
        pauseButtons[currentButtonIndex].onClick.Invoke();
    }
    public void ResumeScreen()
    {
        playerInput.PauseMenu.Disable();
        playerInput.Player.Enable();
        pausePage.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Sayonara Son!");
    }
}