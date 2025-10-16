using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject highScoreText;
    public IntVariable gameScore;

    void Start()
    {
        Debug.Log($"Menu HS: {gameScore.previousHighestValue}");
        UpdateHighScore();
    }
    public void Play()
    {
        SceneManager.LoadScene("Scene 1");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetHighScore()
    {
        gameScore.ResetHighestValue();
        UpdateHighScore();
    }

    public void UpdateHighScore()
    {
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + gameScore.previousHighestValue.ToString();
    }

}
