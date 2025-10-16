using System.Collections.Generic;
using UnityEngine;

public class ResetOrchestrator : MonoBehaviour
{
    List<IReset> resettables;

    void Awake()
    {
        // Include inactive objects within the loaded scene
        var behaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        resettables = new List<IReset>();
        foreach (var mb in behaviours)
        {
            if (!mb) continue;
            var go = mb.gameObject;
            if (!go.scene.IsValid() || !go.scene.isLoaded) continue; // skip assets/prefabs
            if (mb is IReset r && !resettables.Contains(r)) resettables.Add(r);
        }
    }

    public void ResetScene()
    {
        Debug.Log($"[Orchestrator] resetting {resettables.Count}");
        foreach (var r in resettables) r?.ResetState();
    }
}
