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
    public UnityEvent gameOver;

    [SerializeField] private int score = 0;

    override public void Awake()
    {
        base.Awake(); // Call Singleton's Awake
    }
    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    private void OnSceneChanged(Scene curr, Scene next)
    {
        gameStart?.Invoke();
        scoreChange?.Invoke(score);  // keep HUD in sync on new scene
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        Time.timeScale = 1.0f;
        gameRestart.Invoke();
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
    public void ResetScore() { score = 0; scoreChange?.Invoke(score); }
}
