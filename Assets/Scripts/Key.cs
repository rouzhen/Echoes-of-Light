using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource keySound;
    public int keyValue = 1;

    // Track original state
    private SpriteRenderer key;
    private Collider2D keycollider;

    void Awake()
    {
        key = GetComponent<SpriteRenderer>();
        keycollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collected the key
        if (other.CompareTag("Player"))
        {
            CollectKey(other.gameObject);
        }
    }

    void CollectKey(GameObject player)
    {
        Debug.Log("Key collected!");
        // Add key to player's inventory
        KeyInventory inventory = player.GetComponent<KeyInventory>();
        if (inventory != null) inventory.AddKey();

        if (keySound != null) keySound.Play();

        // Hide visuals and disable pickup right away
        if (keycollider) keycollider.enabled = false;
        if (key) key.enabled = false;

        // Deactivate after sound has time to start (or fully finish)
        StartCoroutine(DeactivateAfterDelay());
    }

    private System.Collections.IEnumerator DeactivateAfterDelay()
    {
        // if you want to wait for the full clip:
        float delay = (keySound != null && keySound.clip != null) ? keySound.clip.length : 0.1f;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    public void ResetKey()
    {
        // Show again and re-enable pickup
        gameObject.SetActive(true);
        if (keycollider != null) keycollider.enabled = true;
        if (key != null) key.enabled = true;
    }
}
