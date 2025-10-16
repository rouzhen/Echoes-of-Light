using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // 🔹 Singleton instance
    public static AudioManager Instance { get; private set; }

    [Header("Mixer + Sources")]
    public AudioMixer mixer;           // Assign  AudioMixer in the Inspector
    public AudioSource musicSourceA;   // Assign music source 1
    public AudioSource musicSourceB;   // Assign music source 2
    public AudioSource sfxSource;      // Assign SFX source

    // Optional names of exposed parameters in your mixer
    private const string MUSIC_VOL_PARAM = "MusicVol";
    private const string SFX_VOL_PARAM = "SFXVol";

    // Internals
    private AudioSource activeMusicSource;
    private bool isFading = false;

    // ──────────────────────────────────────────────────────────────
    private void Awake()
    {
        // 🔸 Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // pick initial active source
        activeMusicSource = musicSourceA;

        // Load saved volume settings
        if (PlayerPrefs.HasKey(MUSIC_VOL_PARAM))
            SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOL_PARAM));
        if (PlayerPrefs.HasKey(SFX_VOL_PARAM))
            SetSFXVolume(PlayerPrefs.GetFloat(SFX_VOL_PARAM));
    }

    // ──────────────────────────────────────────────────────────────
    #region MUSIC
    /// <summary>Plays a new music track, crossfading from the previous one.</summary>
    public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
    {
        if (clip == null) return;
        if (isFading) StopAllCoroutines();

        // pick the inactive source
        AudioSource newSource = (activeMusicSource == musicSourceA) ? musicSourceB : musicSourceA;
        newSource.clip = clip;
        newSource.loop = true;
        newSource.volume = 0f;
        newSource.Play();

        StartCoroutine(FadeMusic(activeMusicSource, newSource, fadeDuration));
        activeMusicSource = newSource;
    }

    /// <summary>Stops the current music immediately.</summary>
    public void StopMusic()
    {
        if (activeMusicSource != null)
            activeMusicSource.Stop();
    }

    private IEnumerator FadeMusic(AudioSource from, AudioSource to, float duration)
    {
        isFading = true;
        float t = 0f;
        float fromVol = from ? from.volume : 1f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = t / duration;
            if (to) to.volume = Mathf.Lerp(0f, 1f, a);
            if (from) from.volume = Mathf.Lerp(fromVol, 0f, a);
            yield return null;
        }
        if (from) from.Stop();
        if (to) to.volume = 1f;
        isFading = false;
    }
    #endregion
    // ──────────────────────────────────────────────────────────────
    #region SFX
    /// <summary>Plays a one-shot sound effect.</summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }
    #endregion
    // ──────────────────────────────────────────────────────────────
    #region MIXER CONTROL
    /// <summary>Set music volume from a slider (0–1 range).</summary>
    public void SetMusicVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(MUSIC_VOL_PARAM, dB);
        PlayerPrefs.SetFloat(MUSIC_VOL_PARAM, sliderValue);
    }

    /// <summary>Set SFX volume from a slider (0–1 range).</summary>
    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(SFX_VOL_PARAM, dB);
        PlayerPrefs.SetFloat(SFX_VOL_PARAM, sliderValue);
    }

    /// <summary>Get current slider values (for initializing UI).</summary>
    public float GetSavedMusicVolume() => PlayerPrefs.GetFloat(MUSIC_VOL_PARAM, 1f);
    public float GetSavedSFXVolume() => PlayerPrefs.GetFloat(SFX_VOL_PARAM, 1f);
    #endregion
}
