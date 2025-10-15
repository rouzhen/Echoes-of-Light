using TMPro;
using UnityEngine;

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
    public GameObject restartButton;
    public IntVariable gameScore;
    public GameObject gameOverScreen;
    private RectTransform scoreTextRect;
    private RectTransform restartButtonRect;
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        Debug.Log("GameStart called!");
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
        gameOverScreen.SetActive(true);
        scoreTextRect.anchoredPosition = scoreTextPosition[1];
        restartButtonRect.anchoredPosition = restartButtonPosition[1];
        
    }

}
