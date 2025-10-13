// Mushroom.cs
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public AudioSource pickupSound;

    private SpriteRenderer visuals;
    private Collider2D triggerCol;

    void Awake()
    {
        visuals = GetComponent<SpriteRenderer>();
        triggerCol = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Try player-local LevelUp (EchoMovement)
        var movement = other.GetComponent<EchoMovement>();
        if (movement != null)
        {
            //movement.LevelUp();
        }
        
        if (pickupSound != null) pickupSound.Play();

        // Hide/disable immediately like Key.cs, then deactivate after a short delay
        if (triggerCol != null) triggerCol.enabled = false;
        if (visuals != null) visuals.enabled = false;

        StartCoroutine(DeactivateAfterDelay());
    }

    private System.Collections.IEnumerator DeactivateAfterDelay()
    {
        float delay = (pickupSound != null && pickupSound.clip != null) ? pickupSound.clip.length : 0.1f;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
