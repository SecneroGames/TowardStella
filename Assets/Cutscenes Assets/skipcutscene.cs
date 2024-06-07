using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class skipcutscene : MonoBehaviour
{
    public Button yourButton; // Reference to the button
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
        Debug.Log("Skipp");
        SceneManager.LoadScene(targetSceneName);
    }
}