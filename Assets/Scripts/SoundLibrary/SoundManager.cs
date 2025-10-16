using System.Numerics;
using UnityEngine;

// Singleton class that manages sound effects across scenes
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    // reference sound library
    // private means no other class can access this
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, UnityEngine.Vector3 pos)
    {
        if (clip != null)
        {
            // Spawn audio source at that position and play that sfx
            // If far away, hard to hear
            // Good for sounds not existing in sound library
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName, UnityEngine.Vector3 pos)
    {
        // Delegates to original PlaySound3D method and access sound from sfx library
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }


}
