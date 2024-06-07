using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class skipcutscene : MonoBehaviour
{
    public Button yourButton; // Reference to the button
    public Image fadeImage; // Reference to the image used for fading
    public float fadeDuration = 1f; // Duration of the fade effect
    public string targetSceneName; // Name of the target scene

    void Start()
    {
        // Ensure yourButton and fadeImage are assigned, otherwise try to find them
        if (yourButton == null)
        {
            yourButton = GetComponent<Button>();
        }

        // Add the OnClick listener to your button
        yourButton.onClick.AddListener(StartFade);
    }

    void StartFade()
    {
        StartCoroutine(FadeAndChangeScene());
    }

    IEnumerator FadeAndChangeScene()
    {
        // Fade to black
        yield return StartCoroutine(Fade(1));

        // Load the specified target scene
        SceneManager.LoadScene(targetSceneName);
    }

    IEnumerator Fade(float targetAlpha)
    {
        Color color = fadeImage.color;
        float alpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float blend = Mathf.Clamp01(t / fadeDuration);
            color.a = Mathf.Lerp(alpha, targetAlpha, blend);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}