using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class SampleMushroom : BasePowerup
{
    [Header("Pickup FX")]
    [SerializeField] private AudioSource pickupSound;

    [SerializeField] private SpriteRenderer visuals;

    [SerializeField] private float consumeDelayFallback = 0.1f;

    private Collider2D triggerCol;
    private bool armed;
    private float spawnTime;

    protected override void Start()
    {
        base.Start();
        if (!visuals) visuals = GetComponent<SpriteRenderer>();
        triggerCol = GetComponent<Collider2D>();
        if (triggerCol) triggerCol.isTrigger = true;
    }

    private void OnEnable()
    {
        // Optional: if you removed spawned/hasSpawned, you can drop this entirely
        // spawned = true;
        consumed = false;

        if (triggerCol) triggerCol.enabled = true;
        if (visuals) visuals.enabled = true;

        spawnTime = Time.unscaledTime;
        armed = false;
        StartCoroutine(ArmAfter(0.15f));
    }

    private IEnumerator ArmAfter(float seconds)
    {
        float t = 0f;
        while (t < seconds) { t += Time.unscaledDeltaTime; yield return null; }
        armed = true;
    }

    public override void ApplyPowerup(MonoBehaviour user)
    {
        if (consumed || user == null) return;

        var movement = user.GetComponent<EchoMovement>();
        if (movement == null) return;

        movement.LevelUp();
        consumed = true;

        if (pickupSound != null)
        {
            pickupSound.transform.SetParent(null, true);
            pickupSound.Play();
        }

        if (triggerCol) triggerCol.enabled = false;
        if (visuals) visuals.enabled = false;

        float delay = (pickupSound != null && pickupSound.clip != null) ? pickupSound.clip.length : consumeDelayFallback;
        StartCoroutine(DeactivateAfterDelay(delay));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        float t = 0f;
        while (t < delay)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        if (pickupSound != null) Destroy(pickupSound.gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!armed || consumed) return;
        if (!other.CompareTag("Player")) return;

        var user = other.GetComponentInParent<EchoMovement>();
        if (user == null) return;

        ApplyPowerup(user);
    }
}
