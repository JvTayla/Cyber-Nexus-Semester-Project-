using UnityEngine;

public class SoundScript : MonoBehaviour
{
    // GameObjects for each sound
    public GameObject backgroundMusic;
    public GameObject warningSound;
    public GameObject[] voiceNotes = new GameObject[10];  // Array to store 10 voice notes
    public GameObject alarmSound;
    public GameObject bigRobotWalkSound;
    public GameObject smallRobotWalkSound;
    public GameObject dropSound;
    public GameObject computerSound;
    public GameObject loseMusic;

    public GameObject accessGranted;
    // Function to play background music
    public void PlayBackgroundMusic()
    {
        if (!backgroundMusic.activeSelf)
        {
            backgroundMusic.SetActive(true);
        }
    }

    // Function to stop background music
    public void StopBackgroundMusic()
    {
        if (backgroundMusic.activeSelf)
        {
            backgroundMusic.SetActive(false);
        }
    }
    
    public void PlayAccessGrantedSound()
    {
        accessGranted.SetActive(true);
    }

    // Function to play the warning sound
    public void PlayWarningSound()
    {
        warningSound.SetActive(true);
    }

    // Function to stop the warning sound
    public void StopWarningSound()
    {
        warningSound.SetActive(true);
    }

    // Function to play a specific voice note (index 0-9)
    public void PlayVoiceNote(int index)
    {
        if (index >= 0 && index < voiceNotes.Length)
        {
            voiceNotes[index].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid voice note index.");
        }
    }

    // Function to stop a specific voice note (index 0-9)
    public void StopVoiceNote(int index)
    {
        if (index >= 0 && index < voiceNotes.Length)
        {
            voiceNotes[index].SetActive(false);
        }
    }

    // Function to play the alarm sound
    public void PlayAlarmSound()
    {
        alarmSound.SetActive(true);
    }

    // Function to stop the alarm sound
    public void StopAlarmSound()
    {
        alarmSound.SetActive(false);
    }

    // Function to play the big robot walking sound
    public void PlayBigRobotWalkSound()
    {
        bigRobotWalkSound.SetActive(true);
    }

    // Function to stop the big robot walking sound
    public void StopBigRobotWalkSound()
    {
        bigRobotWalkSound.SetActive(false);
    }

    // Function to play the small robot walking sound
    public void PlaySmallRobotWalkSound()
    {
        smallRobotWalkSound.SetActive(true);
    }

    // Function to stop the small robot walking sound
    public void StopSmallRobotWalkSound()
    {
        smallRobotWalkSound.SetActive(false);
    }

    // Function to play the drop sound
    public void PlayDropSound()
    {
        dropSound.SetActive(true);
    }

    // Function to stop the drop sound
    public void StopDropSound()
    {
        dropSound.SetActive(false);
    }

    // Function to play the computer sound
    public void PlayComputerSound()
    {
        computerSound.SetActive(true);
    }

    // Function to stop the computer sound
    public void StopComputerSound()
    {
        computerSound.SetActive(false);
    }

    // Function to play the lose music
    public void PlayLoseMusic()
    {
        loseMusic.SetActive(true);
    }

    // Function to stop the lose music
    public void StopLoseMusic()
    {
        loseMusic.SetActive(false);
    }
    
    public GameObject GetChildOfVoiceRecorder()
    {
        // Find the parent object with the tag "VoiceRecorder"
        GameObject voiceRecorderObject = GameObject.FindGameObjectWithTag("VoiceRecorder");
        
        if (voiceRecorderObject != null)
        {
            // Get the first child of the found object
            Transform childTransform = voiceRecorderObject.transform.GetChild(0);
            return childTransform != null ? childTransform.gameObject : null; // Return the child GameObject or null if it doesn't exist
        }
        else
        {
            Debug.LogWarning("No GameObject found with the tag 'VoiceRecorder'.");
            return null; // Return null if no object found
        }
    }

    // Function to disable the child of the object with the tag "VoiceRecorder"
    public void DisableChildOfVoiceRecorder()
    {
        GameObject child = GetChildOfVoiceRecorder();

        if (child != null)
        {
            child.SetActive(false); // Disable the child GameObject
        }
        else
        {
            Debug.LogWarning("No child found to disable.");
        }
    }

}
