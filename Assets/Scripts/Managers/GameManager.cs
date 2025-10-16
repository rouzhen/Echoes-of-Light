using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent pauseGame;
    public UnityEvent resumeGame;
    public UnityEvent<int> scoreChange;
    public IntVariable gameScore;
    public PowerupStateSO powerupState;
    bool IsNewSession = true;
    public UnityEvent gameOver;
    [SerializeField] ResetOrchestrator orchestrator;
    [SerializeField] private int score = 0;

    override public void Awake()
    {
        base.Awake(); // Call Singleton's Awake
        Debug.Log($"[GM] SO ref: {powerupState.name} id={powerupState.GetInstanceID()}");
        Debug.Log($"[GM.Awake] pre IsNewSession={IsNewSession} Value={powerupState.Value}");
        if (IsNewSession)
        {
            powerupState.ResetHighestPowerup();
            IsNewSession = false;
            Debug.Log("[GM.Awake] RESET APPLIED");
        }
        else Debug.Log("[GM.Awake] NO RESET");
        Debug.Log($"[GM.Awake] post IsNewSession={IsNewSession} Value={powerupState.Value}");
    }
    void Start()
    {
        Debug.Log("GameManager Start called");
        Debug.Log("[GM.Start] Value now: " + powerupState.Value);
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    private void OnSceneChanged(Scene curr, Scene next)
    {
        ResetScore();
        // SetScore(gameScore.Value);
        gameStart?.Invoke();
        scoreChange?.Invoke(score);  // keep HUD in sync on new scene
        Debug.Log($"[GM.OnSceneChanged] to {next.name} Value={powerupState.Value}");


        // Ensure that buttons are being reset correctly
        var restartButton = GameObject.Find("RestartButton")?.GetComponent<Button>();
        if(restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(GameRestart);
            Debug.Log("[GM.OnSceneChanged] RestartButton reconnected");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        Time.timeScale = 1.0f;
        ResetScore();
        gameRestart.Invoke();
        var orchestrator = FindFirstObjectByType<ResetOrchestrator>();
        orchestrator?.ResetScene();
        Debug.Log($"[GM] Orchestrator={orchestrator?.name}, scene={orchestrator?.gameObject.scene.name}");
    }

    public void RestartToFirstScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene 1", LoadSceneMode.Single);
    }

    

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseGame.Invoke();
        Debug.Log("Game is paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        resumeGame.Invoke();
    }

    public void BackToMainMenuScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void IncreaseScore(int inc)
    {
        SetScore(score + inc); 
        gameScore.ApplyChange(inc); // add to IntVariable
        Debug.Log($"Added {inc} points to gameScore. \nCurrent gameScore: {gameScore.Value}");
    }
    public void SetScore(int newScore)
    {
        score = Mathf.Max(0, newScore);
        scoreChange?.Invoke(score);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }


    public int CurrentScore => score;
    public void ResetScore()
    {
        score = 0; scoreChange?.Invoke(score);
        gameScore.SetValue(0);
        Debug.Log($"Game score reset to {score}. \nHighest score: {gameScore.previousHighestValue.ToString()}");
    }
    
}
