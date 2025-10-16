using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(119, -46, 0),
        new Vector3(16.1f,5.3f,0)
    };
    private Vector3[] restartButtonPosition = {
        new Vector3(-30f, -29f, 0),
        new Vector3(0, -150, 0)
    };

    public GameObject scoreText;
    public GameObject finalScoreText;
    public GameObject highScoreText;
    public GameObject restartButton;
    public GameObject pauseButton;
    public GameObject resumeButton;
    public IntVariable gameScore;
    public GameObject gameOverScreen;
    public GameObject pauseMenuScreen;
    private RectTransform scoreTextRect;
    private RectTransform restartButtonRect;
    private int highScore;
    void Awake()
    {
        //Debug.Log("HUDManager Awake called");
        scoreTextRect = scoreText.GetComponent<RectTransform>();
        restartButtonRect = restartButton.GetComponent<RectTransform>();

        if (GameManager.instance != null)
        {
            GameManager.instance.gameStart.AddListener(GameStart);
            GameManager.instance.gameOver.AddListener(GameOver);
            GameManager.instance.gameRestart.AddListener(GameStart);
            GameManager.instance.scoreChange.AddListener(SetScore);
            GameManager.instance.pauseGame.AddListener(GamePaused);
            GameManager.instance.resumeGame.AddListener(GameResumed);
        }
        else
        {
            Debug.LogError("GameManager.instance is NULL!"); // ✅ ADD THIS
        }

    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.gameStart.RemoveListener(GameStart);
            GameManager.instance.gameOver.RemoveListener(GameOver);
            GameManager.instance.gameRestart.RemoveListener(GameStart);
            GameManager.instance.scoreChange.RemoveListener(SetScore);
        }
    }

    public void GameStart()
    {
        Debug.Log("GameStart called!");
        SetScore(GameManager.instance.CurrentScore);
        Debug.Log($"Highest Score: {gameScore.previousHighestValue.ToString()}");
        Debug.Log($"Current Score: {gameScore.Value.ToString()}");
        // hide gameover panel
        gameOverScreen.SetActive(false);
        scoreTextRect.anchoredPosition = scoreTextPosition[0];
        restartButtonRect.anchoredPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        Debug.Log($"SetScore called with: {score}");
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    
    public void GameOver()
    {
        finalScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameScore.Value.ToString();
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + gameScore.previousHighestValue.ToString();
        gameOverScreen.SetActive(true);
        scoreTextRect.anchoredPosition = scoreTextPosition[1];
        restartButtonRect.anchoredPosition = restartButtonPosition[1];

    }

    public void GamePaused()
    {
        pauseMenuScreen.SetActive(true);
    }
    
    public void GameResumed()
    {
        pauseMenuScreen.SetActive(false);
    }

}
