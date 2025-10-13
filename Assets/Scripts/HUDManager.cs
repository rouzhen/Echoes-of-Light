using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(119, -46, 0),
        new Vector3(16.1f,5.3f,0)
    };
    private Vector3[] restartButtonPosition = {
        new Vector3(2.00002f, -44.5f, 0),
        new Vector3(0, -150, 0)
    };

    public GameObject scoreText;
    public Transform restartButton;

    public GameObject gameOverScreen;
    void Awake()
    {
        Debug.Log("HUDManager Awake called");
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
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        Debug.Log($"SetScore called with: {score}");
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
    }

}
