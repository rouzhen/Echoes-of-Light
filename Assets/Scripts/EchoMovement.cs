using UnityEngine;

public class EchoMovement : MonoBehaviour
{
    public float speed = 10;
    public float upSpeed = 10;
    public float maxSpeed = 20;
    public float doubleJumpForce = 8f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public ParticleSystem ImpactEffect; 
    public Transform footsteps; 
    private Rigidbody2D echoBody;
    private bool stopMoving = false;
    private bool isGroundedBool  = true;
    private bool canDoubleJump = true;
    private bool faceRightState = true;
    private bool wasOnGround = false;
    private SpriteRenderer echoSprite;

    void Start()
    {
        Application.targetFrameRate = 30;
        echoBody = GetComponent<Rigidbody2D>();
        echoSprite = GetComponent<SpriteRenderer>();
    }

        // Remove the old OnEnter2D method and replace with this ground check
    private bool IsGrounded()
    {
        float groundCheckDistance = 0.1f;
        // Cast a ray from the bottom center of the player
        Vector2 rayOrigin = new Vector2(groundCheck.transform.position.x, groundCheck.transform.position.y - 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
}

    private void Jump(float jumpForce)
    {
        echoBody.linearVelocity = new Vector2(echoBody.linearVelocity.x, 0); // Reset Y velocity
        echoBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        // Update grounded status using raycast
        isGroundedBool = IsGrounded();
        Debug.Log("IsGrounded:" + isGroundedBool);
        // Double Jump System
        if (isGroundedBool)
        {
            canDoubleJump = true; // Reset double jump when grounded

            // Regular jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump(upSpeed);
            }
        }
        else
        {
            // Double jump in air
            if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                Jump(doubleJumpForce);
                canDoubleJump = false; // Disable until grounded again
            }
        }

        // Toggle sprite direction
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            echoSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            echoSprite.flipX = false;
        }

        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            stopMoving = true;
        }

        // Landing Impact Effect
        if (!wasOnGround && isGroundedBool && ImpactEffect != null && footsteps != null)
        {
            ImpactEffect.gameObject.SetActive(true);
            ImpactEffect.Stop();
            ImpactEffect.transform.position = new Vector2(footsteps.transform.position.x, footsteps.transform.position.y - 0.2f);
            ImpactEffect.Play();
        }

        wasOnGround = isGroundedBool; // Update previous ground state
    }


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (stopMoving)
        {
            echoBody.linearVelocity = Vector2.zero;
            stopMoving = false;
            return;
        }

        if (moveHorizontal == 0)
            return;

        Vector2 movement = new Vector2(moveHorizontal, 0);

        if (echoBody.linearVelocity.sqrMagnitude < maxSpeed * maxSpeed)
        {
            echoBody.AddForce(movement * speed);
        }
    }
}
