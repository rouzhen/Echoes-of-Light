using UnityEngine;

public class Door : MonoBehaviour, IReset
{
    [Header("Door Settings")]
    public bool isLocked = true;
    public AudioSource unlockSound;
    public bool destroyWhenOpen = false;
    private Collider2D col;
    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Something hit the door: " + collision.gameObject.name);
        // When player touches the door
        if (collision.gameObject.CompareTag("Player"))
        {
            KeyInventory inventory = collision.gameObject.GetComponent<KeyInventory>();

            // Check if player has a key
            if (inventory != null)
            {
                Debug.Log("Has keys: " + inventory.GetKeyCount());
                if ((inventory.HasKey()))
                {
                    OpenDoor(inventory);
                }
                else
                {
                    Debug.Log("You need a key to pass through this door!");
                }
            }
            else
            {
                Debug.LogError("KeyInventory not found on player!");
            }
        }
    }

    void OpenDoor(KeyInventory inventory)
    {
        Debug.Log("Door unlocked!");

        // Use one key
        inventory.UseKey();
        isLocked = false;

        // Play unlock sound
        if (unlockSound != null)
        {
            unlockSound.Play();
        }

        // Make door passable
        if (destroyWhenOpen)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (col != null) col.enabled = false;
            //if (sr != null) sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }
    }

    // Call this from GameManager.GameRestart via UnityEvent
    public void ResetDoor()
    {
        isLocked = true;

        // If it was deactivated, bring it back
        gameObject.SetActive(true);

        // Restore collider and visuals
        if (col != null) col.enabled = true;
        //if (sr != null) sr.color = originalColor;
    }
    public void ResetState() => ResetDoor();
}
