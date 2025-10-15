using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    // Example state variables
    public bool isBigPlayer = false;
    public int score = 0;
    public int lives = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Only one instance allowed
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }
}
