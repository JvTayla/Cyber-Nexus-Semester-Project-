
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConditionalVideoPlaylist : MonoBehaviour
{
    public VideoPlayer videoPlayer;               // VideoPlayer component
    public List<VideoClip> videoClips;            // List of video clips
    public GameObject EndScreen;                  // GameObject to display after all videos finish

    private int currentClipIndex = 0;

    void Start()
    {
        // Ensure EndScreen is hidden initially
        if (EndScreen != null)
        {
            EndScreen.SetActive(false);
        }

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void PlayVideos()
    {
        if (videoClips.Count == 1 && videoPlayer != null /*&& EndScreen != null*/)
        {
            // Start by playing the first video clip
            PlayClip(currentClipIndex);
        }
        else
        {
            Debug.LogWarning("Please assign exactly 10 video clips, a Video Player, and an End Screen GameObject.");
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

    void OnVideoEnd(VideoPlayer vp)
    {
        currentClipIndex++;

        if (currentClipIndex < videoClips.Count) // Check if there are more clips to play
        {
            PlayClip(currentClipIndex); // Play the next video
        }
        else
        {
            // All video clips have finished, show the End Screen
            videoPlayer.Stop();
            EndScreen.SetActive(true); // Activate the EndScreen GameObject
            Debug.Log("All video clips have finished playing. End Screen is now visible.");
        }
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}