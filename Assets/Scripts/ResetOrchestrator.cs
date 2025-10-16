using System.Collections.Generic;
using UnityEngine;

public class ResetOrchestrator : MonoBehaviour
{
    /*private List<IReset> resettables;

    void Awake()
    {
        CaptureAuto(); // or call from Start if spawners run in Awake
    }

    // Call this again if you spawn/destroy pickups dynamically and want to refresh the set.
    public void CaptureAuto()
    {
        resettables = new List<IReset>();

        // Includes inactive objects; must filter to scene instances
        var all = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        foreach (var mb in all)
        {
            if (!mb) continue;
            var go = mb.gameObject;

            // Keep only loaded scene instances; exclude assets/prefabs
            if (!go.scene.IsValid() || !go.scene.isLoaded) continue;

            if (mb is IReset r && !resettables.Contains(r))
                resettables.Add(r);
        }

        Debug.Log($"[Orchestrator] captured {resettables.Count} resettables in {gameObject.scene.name}");
    }

    public void ResetScene()
    {
        if (resettables == null)
        {
            Debug.LogWarning("[Orchestrator] resettables is null; did CaptureAuto run?");
            return;
        }

        Debug.Log($"[Orchestrator] resetting {resettables.Count}");
        foreach (var r in resettables)
        {
            if (r != null) r.ResetState();
        }
    }*/


    [SerializeField] private List<MonoBehaviour> manualResettables = new List<MonoBehaviour>();

    // Optional: keep auto-discovery side-by-side for later
    // [SerializeField] private bool useAutoDiscovery = false;
    // private List<IReset> discovered = new List<IReset>();

    void Awake()
    {
        // If you still want to log what’s in the manual list:
        Debug.Log($"[Orchestrator] manual list has {manualResettables.Count} entries");

        // If you want to validate entries implement IReset at load time:
        for (int i = manualResettables.Count - 1; i >= 0; i--)
        {
            var mb = manualResettables[i];
            if (mb == null || !(mb is IReset))
            {
                Debug.LogWarning($"[Orchestrator] Removing non-IReset or null entry at index {i} ({mb?.name ?? "null"})");
                manualResettables.RemoveAt(i);
            }
        }
        Debug.Log($"[Orchestrator] validated manual IReset count = {manualResettables.Count}");
    }

    public void ResetScene()
    {
        Debug.Log($"[Orchestrator] resetting {manualResettables.Count} (manual)");
        foreach (var mb in manualResettables)
        {
            if (mb == null) continue;
            var r = mb as IReset;
            if (r == null) continue;
            r.ResetState();
        }
    }
}
