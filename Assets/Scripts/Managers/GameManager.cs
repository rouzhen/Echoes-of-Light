using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;

    public IntVariable gameScore;
    public PowerupStateSO powerupState;
    bool IsNewSession = true;
    public UnityEvent gameOver;

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
        gameStart?.Invoke();
        scoreChange?.Invoke(score);  // keep HUD in sync on new scene
        Debug.Log($"[GM.OnSceneChanged] to {next.name} Value={powerupState.Value}");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        Time.timeScale = 1.0f;
        gameRestart.Invoke();
        //IsNewSession = true;
    }

    public void IncreaseScore(int inc) 
    {
        SetScore(score + inc); 
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
    }
}
