
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConditionalVideoPlaylist : MonoBehaviour
{
    public VideoPlayer videoPlayer;               // VideoPlayer component
    public List<VideoClip> videoClips;            // List of video clips
    public Button documentOpenButton;             // Document Open button
    public Button downloadDataButton;             // Download Data button

    private int currentClipIndex = 0;
    private bool isWaitingForDocumentOpen = false;
    private bool isWaitingForDownloadData = false;

    void Start()
    {
        if (videoClips.Count == 10 && videoPlayer != null && documentOpenButton != null && downloadDataButton != null)
        {
            // Start by playing the first video clip
            PlayClip(currentClipIndex);

            // Subscribe to events
            videoPlayer.loopPointReached += OnVideoEnd;
            documentOpenButton.onClick.AddListener(OnDocumentOpenButtonPressed);
            downloadDataButton.onClick.AddListener(OnDownloadDataButtonPressed);

            // Hide both buttons initially
            documentOpenButton.gameObject.SetActive(false);
            downloadDataButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Please assign all 10 video clips, a Video Player, and both buttons.");
        }
    }

    void PlayClip(int index)
    {
        if (index < videoClips.Count)
        {
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();
        }
    }

    // This method is called when a video ends
    void OnVideoEnd(VideoPlayer vp)
    {
        currentClipIndex++;

        if (currentClipIndex == 3) // After Clip #3, show Document Open button and loop
        {
            isWaitingForDocumentOpen = true;
            videoPlayer.isLooping = true;  // Loop Clip #3
            documentOpenButton.gameObject.SetActive(true);
        }
        else if (currentClipIndex == 4) // After Clip #4, show Download Data button and play next clip
        {
            isWaitingForDownloadData = true;
            downloadDataButton.gameObject.SetActive(true);
            PlayClip(currentClipIndex); // Play Clip #4
        }
        else if (currentClipIndex >= 5 && currentClipIndex < videoClips.Count) // Clips #5 to #9
        {
            PlayClip(currentClipIndex);
        }
        else if (currentClipIndex >= videoClips.Count) // After Clip #9, finish the playlist
        {
            Debug.Log("Playlist finished.");
            documentOpenButton.gameObject.SetActive(false);
            downloadDataButton.gameObject.SetActive(false);
            videoPlayer.Stop(); // Stop playback after finishing all clips
        }
    }

    void OnDocumentOpenButtonPressed()
    {
        if (isWaitingForDocumentOpen)
        {
            isWaitingForDocumentOpen = false;
            documentOpenButton.gameObject.SetActive(false); // Hide the button

            // Advance to the next clip (Clip #4)
            currentClipIndex++;
            videoPlayer.isLooping = false; // Stop looping for Clip #3

            if (currentClipIndex < videoClips.Count)
            {
                PlayClip(currentClipIndex); // Play next clip
            }
        }
    }

    void OnDownloadDataButtonPressed()
    {
        if (isWaitingForDownloadData)
        {
            isWaitingForDownloadData = false;
            downloadDataButton.gameObject.SetActive(false); // Hide the button

            // Advance to the next clip (Clip #5 to #9)
            currentClipIndex++;

            if (currentClipIndex < videoClips.Count)
            {
                PlayClip(currentClipIndex); // Play next clip
                videoPlayer.isLooping = false; // Ensure looping is off for subsequent clips
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events when the object is destroyed to avoid memory leaks
        videoPlayer.loopPointReached -= OnVideoEnd;
        documentOpenButton.onClick.RemoveListener(OnDocumentOpenButtonPressed);
        downloadDataButton.onClick.RemoveListener(OnDownloadDataButtonPressed);
    }
}
