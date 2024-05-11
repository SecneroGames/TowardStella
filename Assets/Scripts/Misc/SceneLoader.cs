using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance; 
    private string currentScene;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(string sceneName, bool async = false,bool isAdditive = false)
    {
        if (async)
        {
            StartCoroutine(LoadSceneAsync(sceneName, isAdditive));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
            currentScene = sceneName;
        }
    }

    IEnumerator LoadSceneAsync(string sceneName, bool isAdditive = false)
    {

        AsyncOperation asyncLoad = (isAdditive) ? SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive) : SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        currentScene = sceneName;
    }

  

    public void UnloadPreviousScene()
    {
        if (!string.IsNullOrEmpty(currentScene))
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}
