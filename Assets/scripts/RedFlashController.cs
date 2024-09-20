using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RedFlashController : MonoBehaviour
{
    public Image screenFlashImage;  // UI Image to handle screen flashing

    private void Start()
    {
        // Ensure the flash image is transparent at the start
        if (screenFlashImage != null)
        {
            screenFlashImage.color = new Color(1f, 0f, 0f, 0f);  // Set the alpha to 0 (transparent)
        }
    }

    // Call this method to trigger the red flash effect
    public void TriggerRedFlash()
    {
        StartCoroutine(FlashRedScreen());
    }

    // Coroutine to handle the red screen flash effect
    private IEnumerator FlashRedScreen()
    {
        if (screenFlashImage != null)
        {
            // Set the image to semi-transparent red (50% opacity) immediately
            screenFlashImage.color = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent red

            // Wait for a short duration (e.g., 0.2 seconds)
            yield return new WaitForSeconds(0.2f);

            // Gradually fade out the red flash over time (e.g., over 0.5 seconds)
            float fadeDuration = 0.5f;
            for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
            {
                // Fade the alpha from 0.5f (50% opacity) to 0f (fully transparent)
                float alpha = Mathf.Lerp(0.5f, 0f, t / fadeDuration);
                screenFlashImage.color = new Color(1f, 0f, 0f, alpha);
                yield return null;
            }

            // Ensure the flash image is completely transparent after the effect
            screenFlashImage.color = new Color(1f, 0f, 0f, 0f);
        }
    }
}
