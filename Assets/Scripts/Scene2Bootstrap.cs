using UnityEngine;

public class Scene2Bootstrap : MonoBehaviour
{
    [SerializeField] private PowerupStateSO powerupState;

    // Runs before Start on all scripts in the scene
    private void Awake()
    {
        Debug.Log($"[Scene2Bootstrap.Awake] SO={powerupState?.name} id={powerupState?.GetInstanceID()} Value={powerupState?.Value}");
    }

    // Optional: also log in OnEnable/Start to see changes over the init lifecycle
    private void OnEnable()
    {
        Debug.Log($"[Scene2Bootstrap.OnEnable] Value={powerupState?.Value}");
    }

    private void Start()
    {
        Debug.Log($"[Scene2Bootstrap.Start] Value={powerupState?.Value}");
    }
}
