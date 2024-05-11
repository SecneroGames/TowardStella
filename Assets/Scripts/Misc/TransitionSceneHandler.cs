using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionSceneHandler : MonoBehaviour
{
    [SerializeField] private PlayerDataSO PlayerStatSO;
    [SerializeField] private TextMeshProUGUI CoinText;

    private void Start()
    {
        AudioManager.instance.PlayBGM("TransitionBGM");
        CoinText.text = PlayerStatSO.Credits.ToString();
    }

    public void GoBackToMainMenu()
    {
        SaveManager.instance.SaveGame();
        SceneLoader.instance.LoadScene("MainMenu");
        SceneLoader.instance.UnloadPreviousScene();
    }

    public void ProceedToRace(string chapter)
    {
        SaveManager.instance.SaveGame();
        SceneLoader.instance.LoadScene(chapter);
    }
}
