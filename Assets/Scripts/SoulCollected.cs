using UnityEngine;

// This is just a text code
public class SoulCollected : MonoBehaviour
{
    GameManager gameManager;

    [System.NonSerialized]
    public int points = 1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        gameManager.IncreaseScore(1);

        Destroy(gameObject);
    }
}
