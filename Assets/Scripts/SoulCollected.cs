using UnityEngine;

// This is just a text code
public class SoulCollected : MonoBehaviour
{

    [System.NonSerialized]
    public int points = 1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CollectSoul();
    }

    void CollectSoul()
    {
        Debug.Log("Soul collected!");

        if (GameManager.instance != null)
        {
            GameManager.instance.IncreaseScore(points);
        }

        gameObject.SetActive(false);
        
    }

    
}
