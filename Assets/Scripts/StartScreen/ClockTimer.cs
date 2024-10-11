using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockTimer : MonoBehaviour
{
    public TextMeshProUGUI minuteCounterText;
    public TextMeshProUGUI minuteCounterText2;
    private float timeSpent; // Total time in seconds
    private float elapsedRealTime; // Time elapsed in real seconds

    private void Start()
    {
        // Start the clock at 12 minutes and 47 seconds
        int startMinute = 12; // Starting minutes
        int startSecond = 47; // Starting seconds
        timeSpent = (startMinute * 60) + startSecond; // Total seconds
    }

    private void Update()
    {
        // Increment elapsedRealTime by the time that has passed since the last frame in seconds
        elapsedRealTime += Time.deltaTime;

        // Check if 10 real seconds have passed
        if (elapsedRealTime >= 10f)
        {
            // Increase timeSpent by 1 second
            timeSpent += 1f;
            elapsedRealTime -= 10f; // Reset elapsedRealTime by subtracting 10 seconds
        }

        // Calculate total minutes and seconds
        int totalMinutes = Mathf.FloorToInt(timeSpent / 60);
        int minutes = totalMinutes % 24; // Reset after 24 minutes
        int seconds = Mathf.FloorToInt(timeSpent % 60);

        // Update the text display
        minuteCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        minuteCounterText2.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

