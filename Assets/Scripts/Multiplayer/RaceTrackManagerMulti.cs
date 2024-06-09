using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RaceTrackManagerMulti : MonoBehaviour
{
    public static RaceTrackManagerMulti Instance;
    [SerializeField] Transform StartingPosition;
    [SerializeField] Transform GoalPosition;

    [Space]
    [SerializeField] GameObject EndRacePanel;
    [SerializeField] GameObject ControlPanel;
    [SerializeField] TextMeshProUGUI WinLoseText;

    public List<ShipTracker> Ships = new List<ShipTracker>();

    bool isRaceOngoing = true;
    float goalOffset = 50f;
    public int shipsDone = 0;

    [SerializeField] UnityEvent OnGoalReached = new UnityEvent();
    [SerializeField] UnityEvent OnRaceStarted = new UnityEvent();

    [SerializeField] private GameObject PausePanel;
    [SerializeField] private TextMeshProUGUI lapsText;
    [SerializeField] private TextMeshProUGUI counter;
    public bool hasLaps;
    public int maxLaps = 3;
    public int laps = 0;
    private bool gameStart = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        Input.multiTouchEnabled = true;
        EndRacePanel.SetActive(false);
        ControlPanel.SetActive(true);
        AudioManager.instance.PlayBGM("RaceBGM");
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if(gameStart)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        lapsText.text = $"Lap {laps}/{maxLaps}";
        if(isRaceOngoing)
        {
            if(hasLaps)
            {
                CheckLaps();
            }
            else
            {
                CheckShips();
            }
        }
        
    }

    private void CheckLaps()
    {
        if(laps >= maxLaps)
        {
            isRaceOngoing = false;
            EndRacePanel.SetActive(true);
            ControlPanel.SetActive(false);
            EndGameDebug();
        }
        else
        {
            
        }
    }

    //Update all ship positions.
    void CheckShips()
    {
        foreach (var ship in Ships)
        {
            if (GoalReached(ship.transform) && ship.enabled)
            {
                if(ship.shipTag == ShipTracker.ShipTag.PLAYER)
                {
                   
                    isRaceOngoing = false;
                    EndRacePanel.SetActive(true);
                    ControlPanel.SetActive(false);
                    EndGameDebug();

                    OnGoalReached?.Invoke();
                }
                else
                {
                    shipsDone++;
                    ship.gameObject.SetActive(false);
                }
              
                ship.enabled = false;
                break;
            }
        }
    }

    void EndGameDebug()
    {
        gameStart = false;
        if (shipsDone <= 0)
        {
            WinLoseText.text = "You Won!";
            AudioManager.instance.PlayBGMOnce("WinSound");
            Debug.Log("You Won!");
        }
        else
        {
            WinLoseText.text = $"Try harder next time.";
            Debug.Log("You lose");
        }
    }

    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //check if a ship has reached goal
    public bool GoalReached(Transform shipTransform)
    {
        var distance = Vector3.Distance(GoalPosition.transform.position, shipTransform.position);
        var direction = GoalPosition.transform.position.z - shipTransform.position.z;
        //Debug.Log($"Distance to goal {distance} - dir {direction}");
        if (direction < 0) return true;
        return distance <= goalOffset;
    }

    public void PauseButton()
    {
        gameStart = false;
        PausePanel.SetActive(true);
    }   

    public void ResumeButton()
    {
        gameStart = true;
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

    private IEnumerator StartCountdown()
    {
        int countdownTime = 3;
        while (countdownTime > 0)
        {
            counter.text = countdownTime.ToString();
            yield return new WaitForSecondsRealtime(1);
            countdownTime--;
        }
        counter.text = "GO!";
        yield return new WaitForSecondsRealtime(1);
        counter.gameObject.SetActive(false);
        gameStart = true;
    }
}

