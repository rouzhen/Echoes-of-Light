using UnityEngine;
// just to check for bugs between scenes
public class Scene2Bootstrap : MonoBehaviour
{
    [SerializeField] private PowerupStateSO powerupState;

    private void Awake()
    {
        Debug.Log($"[Scene2Bootstrap.Awake] SO={powerupState?.name} id={powerupState?.GetInstanceID()} Value={powerupState?.Value}");
    }
    private void OnEnable()
    {
        Debug.Log($"[Scene2Bootstrap.OnEnable] Value={powerupState?.Value}");
    }

    private void Start()
    {
        Debug.Log($"[Scene2Bootstrap.Start] Value={powerupState?.Value}");
    }
}
