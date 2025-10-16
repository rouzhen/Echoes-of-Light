using UnityEngine;


public abstract class BasePowerup : MonoBehaviour, IPowerup, IReset
{
    public PowerupType type;
    //public bool spawned = false;
    protected bool consumed = false;
    protected Rigidbody2D rigidBody;

    private Vector3 initialPos;
    private Quaternion initialRot;
    private Collider2D triggerCol;
    private SpriteRenderer visuals;

    // base methods
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        triggerCol = GetComponent<Collider2D>();
        visuals = GetComponent<SpriteRenderer>();
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    

    // interface methods
    // 1. concrete methods
    public PowerupType powerupType
    {
        get // getter
        {
            return type;
        }
    }

    /*public bool hasSpawned
    {
        get // getter
        {
            return spawned;
        }
    }*/

    public void DeactivatePowerup() 
    {
        consumed = true;
        if (triggerCol) triggerCol.enabled = false;
        if (visuals) visuals.enabled = false;
        gameObject.SetActive(false);
    }
    public virtual void ResetPickup()
    {
        consumed = false;
        transform.SetPositionAndRotation(initialPos, initialRot);

        if (triggerCol) triggerCol.enabled = true;
        if (visuals) visuals.enabled = true;
        gameObject.SetActive(true);
        Debug.Log($"[Pickup] Reset {name}");
    }
    public void ResetState() => ResetPickup();

    // 2. abstract methods, must be implemented by derived classes
    /*public abstract void SpawnPowerup();*/
    public abstract void ApplyPowerup(MonoBehaviour i);
}
