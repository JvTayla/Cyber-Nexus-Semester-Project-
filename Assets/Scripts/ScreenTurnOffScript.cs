using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTurnOffScript : MonoBehaviour
{
    public Image blackScreen;  // Assign the Image component here in the Inspector
    public float fadeDuration = 1f;  // Duration of the fade

    // This function will fade the screen to black
    public void FadeToBlack()
    {
        StartCoroutine(FadeOut());
    }

    // Coroutine to handle the fade effect
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color screenColor = blackScreen.color;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            screenColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);  // Increase alpha over time
            blackScreen.color = screenColor;
            yield return null;  // Wait until the next frame
        }

        screenColor.a = 1;  // Ensure alpha is set to 1 (completely black)
        blackScreen.color = screenColor;
    }
}
