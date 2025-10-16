// Mushroom.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FireFlower : BasePowerup
{
    public PowerupStateSO powerupState; // Assign in Inspector
    public AudioSource pickupSound;
    private SpriteRenderer visuals;
    private Collider2D triggerCol;

    protected override void Start()
    {
        base.Start();
        visuals = GetComponent<SpriteRenderer>();
        triggerCol = GetComponent<Collider2D>();
        if (triggerCol != null) triggerCol.isTrigger = true;
    }

   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (consumed) return;
        if (!other.CompareTag("Player")) return;
        var movement = other.GetComponentInParent<EchoMovement>();
        if (movement == null) return;
        ApplyPowerup(movement); // Use interface method
        consumed = true;
        if (pickupSound != null) {
            pickupSound.transform.SetParent(null, true);
            pickupSound.Play();
        }
        if (triggerCol != null) triggerCol.enabled = false;
        if (visuals != null) visuals.enabled = false;
        StartCoroutine(DeactivateAfterDelay());
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // Update the ScriptableObject state
        powerupState.SetValue(PowerupType.FireFlower);
        Debug.Log($"Powerup state: {powerupState.Value}");

        // Optionally, call a method on EchoMovement to update visuals/collider
        var movement = i as EchoMovement;
        if (movement != null) {
            movement.ApplyForm(powerupState.Value);
            movement.LevelUp(); // Switch to big form
        }
    }

    private System.Collections.IEnumerator DeactivateAfterDelay()
    {
        float delay = (pickupSound != null && pickupSound.clip != null) ? pickupSound.clip.length : 0.1f;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}