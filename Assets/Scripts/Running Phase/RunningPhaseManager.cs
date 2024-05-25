using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RunningPhaseManager : MonoBehaviour
{
    public static RunningPhaseManager instance;

    public Transform Player;
    [Space]
    float GameTimer;
    float TimeLimit = 30f;

    [SerializeField]
    TextMeshProUGUI TimerText;
    [SerializeField]
    TextMeshProUGUI CreditsText;

    [SerializeField] 
    PlayerDataSO shipStats;

    [Space]
    public UnityEvent OnTimerEnd = new UnityEvent();
    bool gameIsRunning = false;


    float CreditsPickupCount = 0;


    [Header("Spawning")]
    public GameObject CreditsPickupObject;
    public GameObject TimePickupObject;

    //my code
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private Image healthImage;
    private float maxHealth = 100;
    private float health = 100;

    private void Awake()
    {
        instance = this;
        GameTimer = TimeLimit;
        gameIsRunning = true;
    }
    void Start()
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlayBGM("RunningBGM");
        CreditsPickupCount = 0;
        playerMovementController.enabled = true;
        CreditsText.text = $"{CreditsPickupCount} Credits";
    }

    private void Update()
    {
        if (gameIsRunning == false) return;

        if(GameTimer > 0)
        {
            GameTimer -= Time.deltaTime;
            TimerText.text = $"{Mathf.CeilToInt(GameTimer).ToString()}s";

            if (GameTimer<0)
            {                
                GameTimer = 0;
                TimerText.text = $"{Mathf.CeilToInt(GameTimer).ToString()}s";
             
            }
        }

        if (GameTimer <= 0 || health <= 0)
        {
            GameOver();
            
            // GameTimer = 0;
            // TimerText.text = $"{Mathf.CeilToInt(GameTimer).ToString()}s";
            // if (CreditsPickupCount >= 0)
            // {
            //     shipStats.Credits += CreditsPickupCount;
            // }
            // OnTimerEnd?.Invoke();
            // SceneLoader.instance.LoadScene("TransitionScene");
            // gameIsRunning = false;
        }

        healthImage.fillAmount = health/maxHealth;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
    }

    public void OnPickup(PickupType type)
    {
        switch (type)
        {
            case PickupType.CREDIT:
                
                CreditsPickupCount += 500f;
                CreditsText.text = $"{CreditsPickupCount} Credits";
                break;
            case PickupType.TIME:
                GameTimer += 5f;
              
                break;
        }

    }

    public string PickupValue(PickupType type)
    {
        string val = string.Empty; // Initialize with a default value
        switch (type)
        {
            case PickupType.CREDIT:
                val = "+500 credits";
                break;
            case PickupType.TIME:
                val = "+5s timer";
                break;
            default:
                val = "Unknown"; // Optional: Handle unexpected cases
                break;
        }
        return val;
    }

    public void OnEnemyHit()
    {
        AudioManager.instance.PlaySFX("DamageSFX");
        Debug.Log("EnemyHit");
        //CreditsPickupCount -= 600f;
        //GameTimer -= 10;
        health -= 15;

        TimerText.text = $"{Mathf.CeilToInt(GameTimer).ToString()}s";
        CreditsText.text = $"{CreditsPickupCount} Credits";
    }
    public void EndGame(string sceneName)
    {
        Debug.Log("End");
        if (CreditsPickupCount >= 0)
        {
            shipStats.Credits += CreditsPickupCount;
        }

        GameTimer = 0;
        TimerText.text = $"{Mathf.CeilToInt(GameTimer).ToString()}s";
        if (CreditsPickupCount >= 0)
        {
            shipStats.Credits += CreditsPickupCount;
        }
        OnTimerEnd?.Invoke();
        SceneLoader.instance.LoadScene(sceneName);
        gameIsRunning = false;

    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        playerMovementController.enabled = false;
        PausePanel.SetActive(true);
    }   

    public void ResumeButton()
    {
        Time.timeScale = 1;
        playerMovementController.enabled = true;
        PausePanel.SetActive(false);
    }   

    public void RestartButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }   

    public void MenuButton()
    {
        SceneLoader.instance.LoadScene("MainMenu");
    }   
}
