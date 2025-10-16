using UnityEngine;
public class UIActions : MonoBehaviour
{
    GameManager gm;
    void Awake() { gm = FindFirstObjectByType<GameManager>(); }
    public void OnRestartClicked() { if (gm) gm.RestartToFirstScene(); }
}
