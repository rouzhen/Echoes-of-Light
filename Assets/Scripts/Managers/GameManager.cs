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
    private static bool hasColdStarted = false;

    override public void Awake()
    {
        base.Awake(); // Call Singleton's Awake
        DontDestroyOnLoad(gameObject);
        Debug.Log($"[GM] SO ref: {powerupState.name} id={powerupState.GetInstanceID()}");

        if (!hasColdStarted)
        {
            gameScore.SetValue(0);
            powerupState.ResetHighestPowerup();  // reset only once, not on every scene load
            hasColdStarted = true;
            Debug.Log("[GM.Awake] Cold start reset applied");
        }
        else
        {
            Debug.Log("[GM.Awake] Continuing existing session, no reset");
        }

        Debug.Log($"[GM.Awake] Powerup Value after Awake: {powerupState.Value}");

        // if (IsNewSession)
        // {
        //     gameScore.SetValue(0);
        //     powerupState.ResetHighestPowerup();
        //     IsNewSession = false;
        //     Debug.Log("[GM.Awake] RESET APPLIED");
        // }
        // else Debug.Log("[GM.Awake] NO RESET");
        // Debug.Log($"[GM.Awake] post IsNewSession={IsNewSession} Value={powerupState.Value}");
    }
    void Start()
    {
        Debug.Log("GameManager Start called");
        Debug.Log("[GM.Start] Value now: " + powerupState.Value);
        Debug.Log("[GM.Start] Current Score: " + gameScore.Value);
        Debug.Log("[GM.Start] High Score: " + gameScore.previousHighestValue);
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        // SceneManager.activeSceneChanged += OnSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameStart?.Invoke();
        scoreChange?.Invoke(score);
        Debug.Log($"[GM.OnSceneLoaded] Scene: {scene.name} Powerup={powerupState.Value}");

        StartCoroutine(ReconnectAllButtons());
    }
    
    private IEnumerator ReconnectAllButtons()
    {
        yield return null; // wait one frame so UI is fully initialized

        Button[] allButtons = FindObjectsOfType<Button>(true); // include inactive
        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveAllListeners();

            switch (btn.name)
            {
                case "RestartButton":
                    btn.onClick.AddListener(GameRestart);
                    break;
                case "ResumeButton":
                    btn.onClick.AddListener(ResumeGame);
                    break;
                case "MenuButton":
                    btn.onClick.AddListener(BackToMainMenuScene);
                    break;
                case "PauseButton":
                    btn.onClick.AddListener(PauseGame);
                    break;
                // add more cases for any other buttons
            }
        }
        Debug.Log($"[GM] Reconnected {allButtons.Length} buttons");
    }


    private void OnSceneChanged(Scene curr, Scene next)
    {
        // ResetScore();
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

    private void OnDestroy()
    {   
     SceneManager.sceneLoaded -= OnSceneLoaded;
    }   

    
}
