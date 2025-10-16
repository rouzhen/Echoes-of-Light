using UnityEngine;
 
 
[CreateAssetMenu(menuName = "Variables/AudioSettings")]
public class AudioSettingsSO : ScriptableObject
{
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;


// In AudioManager.cs
public AudioSettingsSO audioSettings;
    void Start()
    {
        //mixer.SetFloat("MasterVolume", audioSettings.masterVolume);
        // etc.
    }

}