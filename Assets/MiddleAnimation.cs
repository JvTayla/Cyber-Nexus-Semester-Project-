using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MiddleAnimation : MonoBehaviour

{
    public VideoPlayer videoPlayer;          // Reference to the VideoPlayer component
    public VideoClip[] videoClips;           // Array of video clips (size 3 for the first three)
    public GameObject MiddleScreenImage;
    public GameObject UploadButton;
    // Reference to the image you want to show after the videos finish

    private int currentClipIndex = 0;

    void Start()
    {
        // Ensure the last screen image is hidden at the start
        MiddleScreenImage.SetActive(false);
        UploadButton.SetActive(false);

        if (videoClips.Length >= 2)
        {
            // Play the first video clip
            videoPlayer.clip = videoClips[currentClipIndex];
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd; // Subscribe to video end event
        }
        else
        {
            Debug.LogError("Please assign at least 3 video clips.");
        }
    }

    // This method is called when the current video finishes
    void OnVideoEnd(VideoPlayer vp)
    {
        currentClipIndex++;

        // Check if there are more videos to play
        if (currentClipIndex < videoClips.Length)
        {
            // Play the next video
            videoPlayer.clip = videoClips[currentClipIndex];
            videoPlayer.Play();
        }
        else
        {
            // All videos are done, show the last screen image
            ShowMiddleScreenImage();
        }
    }

    // This method is called to display the last screen image
    void ShowMiddleScreenImage()
    {
        // Stop the video playback and set the image as active
        videoPlayer.Stop();
        MiddleScreenImage.SetActive(true);
        UploadButton.SetActive(true);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the script is destroyed to avoid memory leaks
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
