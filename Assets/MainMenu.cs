using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public List<Button> ChapterButtons = new List<Button>();

    private void Start()
    {
        if(!SaveManager.instance.CheckFile())
        {
            SaveManager.instance.NewGame(); // reset values if there is no saved data
        }
        else
        {
            SaveManager.instance.LoadGame();
        }

        // disable chapter buttons
        for (int i = 0; i < ChapterButtons.Count; i++)
        {
            var currentChapter = i + 1;
            Debug.Log($"Chapter {currentChapter} unlocked? {SaveManager.instance.IsChapterUnlocked(currentChapter)}");
            if (SaveManager.instance.IsChapterUnlocked(currentChapter))
            {
                ChapterButtons[i].interactable = true;
             
            }
            else
            {
                ChapterButtons[i].interactable = false;
            }
        }
    }
    public void PlayGame()
    {
        SceneLoader.instance.LoadScene("RunningScene_1");
    }

    public void LoadGameLevel(string chapter)
    {
        //change name to scene to load name
        SceneLoader.instance.LoadScene(chapter);

    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Player has Quit the Game.");
    }

}