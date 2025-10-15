// Mushroom.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Mushroom : MonoBehaviour
{
    public AudioSource pickupSound;

    private SpriteRenderer visuals;
    private Collider2D triggerCol;
    private bool consumed;

    void Awake()
    {
        visuals = GetComponent<SpriteRenderer>();
        triggerCol = GetComponent<Collider2D>();
        if (triggerCol != null) triggerCol.isTrigger = true;

        Debug.Log("[Mushroom] Awake", this);
    }
    void OnEnable()
    {
        consumed = false;
        if (triggerCol != null) triggerCol.enabled = true;
        if (visuals != null) visuals.enabled = true;
        Debug.Log("[Mushroom] Enabled", this);
    }
    
    void Start()
    {
        Debug.Log("[Mushroom] Start at " + transform.position, this);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (consumed) return;
        if (!other.CompareTag("Player")) return;
        var movement = other.GetComponentInParent<EchoMovement>();
        if (movement == null)
        {
            Debug.LogWarning("[Mushroom] Player tag hit, but EchoMovement not found on parent chain.", this);
            return;
        }

        // Apply the effect – return true if it actually did something
        bool applied = TryApply(movement);
        if (!applied)
        {
            Debug.LogWarning("[Mushroom] Apply failed; not consuming pickup.", this);
            return;
        }

        consumed = true;

        if (pickupSound != null)
        {
            pickupSound.transform.SetParent(null, true);
            pickupSound.Play();
        }

        if (triggerCol != null) triggerCol.enabled = false;

        // Hide visuals
        if (visuals != null) visuals.enabled = false;

        StartCoroutine(DeactivateAfterDelay());
    }

    private bool TryApply(EchoMovement movement)
    {
        movement.LevelUp();
        return true;
    }
    private System.Collections.IEnumerator DeactivateAfterDelay()
    {
        float delay = (pickupSound != null && pickupSound.clip != null) ? pickupSound.clip.length : 0.1f;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
      void OnDisable()
    {
        Debug.Log("[Mushroom] Disabled", this);
    }

    void OnDestroy()
    {
        Debug.Log("[Mushroom] Destroyed", this);
    }
}

