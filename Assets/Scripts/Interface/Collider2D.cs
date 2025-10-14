using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerColliderResizer : MonoBehaviour
{
    [Header("Auto-fit settings")]
    public bool useAutoFit = true; // If true, fit collider to sprite bounds
    public Vector2 padding = new Vector2(0.05f, 0.05f);
    public Vector2 sizeScale = Vector2.one;
    public CapsuleDirection2D autoDirection = CapsuleDirection2D.Vertical;

    [Header("Manual override (optional)")]
    public bool useManualProfile = false;
    public Vector2 manualSize = new Vector2(0.6f, 1.3f);
    public Vector2 manualOffset = Vector2.zero;
    public CapsuleDirection2D manualDirection = CapsuleDirection2D.Vertical;

    private SpriteRenderer sr;
    private CapsuleCollider2D capsule;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    // Call this from an Animation Event or when switching states
    public void UpdateColliderToSprite()
    {
        if (useManualProfile)
        {
            capsule.size = manualSize;
            capsule.offset = manualOffset;
            capsule.direction = manualDirection;
        }
        else if (useAutoFit)
        {
            Bounds b = sr.bounds;
            Vector3 lossy = transform.lossyScale;
            float localWidth = ((b.size.x + padding.x) * sizeScale.x) / Mathf.Max(0.0001f, lossy.x);
            float localHeight = ((b.size.y + padding.y) * sizeScale.y) / Mathf.Max(0.0001f, lossy.y);
            capsule.size = new Vector2(localWidth, localHeight);
            capsule.direction = autoDirection;
            Vector3 localCenter = transform.InverseTransformPoint(b.center);
            capsule.offset = new Vector2(localCenter.x, localCenter.y);
        }
        Physics2D.SyncTransforms();
    }
}