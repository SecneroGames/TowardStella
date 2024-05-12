using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RaceTrackManager : MonoBehaviour
{
    
    [SerializeField] Transform StartingPosition;
    [SerializeField] Transform GoalPosition;

    [Space]
    [SerializeField] GameObject EndRacePanel;
    [SerializeField] GameObject ControlPanel;
    [SerializeField] TextMeshProUGUI WinLoseText;

    public List<ShipTracker> Ships = new List<ShipTracker>();

    bool isRaceOngoing = true;
    float goalOffset = 50f;
    int shipsDone = 0;

    [SerializeField] UnityEvent OnGoalReached = new UnityEvent();
    [SerializeField] UnityEvent OnRaceStarted = new UnityEvent();


    //my code
    [SerializeField] private GameObject PausePanel;

    private void Start()
    {
        Time.timeScale = 1;
        Input.multiTouchEnabled = true;
        EndRacePanel.SetActive(false);
        ControlPanel.SetActive(true);
        AudioManager.instance.PlayBGM("RaceBGM");
    }

    private void Update()
    {
        if(isRaceOngoing)
        {
            CheckShips();
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
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }   

    public void ResumeButton()
    {
        Time.timeScale = 1;
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

