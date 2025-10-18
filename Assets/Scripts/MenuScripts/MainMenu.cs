using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject highScoreText;
    public IntVariable gameScore;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
        Debug.Log($"Menu HS: {gameScore.previousHighestValue}");
        UpdateHighScore();
    }
    public void Play()
    {
        SceneManager.LoadScene("Scene 1");
        // MusicManager.Instance.PlayMusic("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetHighScore()
    {
        gameScore.ResetHighestValue();
        Debug.Log($"High score reset. Current High score: {gameScore.previousHighestValue}");
        UpdateHighScore();
    }

    public void UpdateHighScore()
    {
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + gameScore.previousHighestValue.ToString();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

}
