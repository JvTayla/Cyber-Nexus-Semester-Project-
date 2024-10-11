using UnityEngine;
using UnityEngine.SceneManagement; // For handling scene switching
using UnityEngine.UI; // For buttons and UI elements

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;

    void Update()
    {
   
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Unfreeze the game
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        isPaused = true;
    }

    public void LoadControls()
    {
        // Optionally load a separate controls menu
        Debug.Log("Show Controls");
        // You can display controls UI or load another scene
    }

    public void QuitGame()
    {
        // Exit the game or return to the main menu
        Debug.Log("Quit Game");
        Application.Quit(); // For standalone builds
        // SceneManager.LoadScene("MainMenu"); // For switching to main menu scene
    }
}
