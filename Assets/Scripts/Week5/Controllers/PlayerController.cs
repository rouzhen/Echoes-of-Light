using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameConstants gameConstants;
    public PowerupStateSO powerupState;

    private bool onGroundState = true;
    private bool faceRightState = true;

    private bool moving = false;
    private bool jumpedState = false;
    private bool IsLevelingUp = false;
    private bool IsFire = false;

    private Rigidbody2D echoBody;
    private SpriteRenderer echoSprite;

    public Animator echoAnimator;
    public AudioSource echoAudio;
    public AudioClip echoDeath;
    public AudioClip levelUpClip;

    private float speed;
    private float maxSpeed;
    private float deathImpulse;
    private float upSpeed;
    private int fallVelocityThreshold;

    [System.NonSerialized]
    public bool alive = true;
    public Transform gameCamera;
    private int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    void Start()
    {
        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        fallVelocityThreshold = gameConstants.fallVelocityThreshold;
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        echoBody = GetComponent<Rigidbody2D>();
        echoSprite = GetComponent<SpriteRenderer>();
        // update animator state
        echoAnimator.SetBool("onGround", onGroundState);
        echoAnimator.SetBool("IsLevelingUp", IsLevelingUp);
        echoAnimator.SetBool("IsFire", IsFire);
        ApplyForm(powerupState.Value);
        Debug.Log($"---Level start----\n[Player] SO ref: {powerupState.name} id={powerupState.GetInstanceID()}");
        Debug.Log($"---Level start----\n[Player.Start] Value={powerupState.Value}");
    }

    // Update is called once per frame
    void Update()
    {
        echoAnimator.SetFloat("xSpeed", Mathf.Abs(echoBody.linearVelocity.x));
    }

    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }

        if (echoBody.linearVelocityY < fallVelocityThreshold)
        {
            FallDetector();
        }
    }

    void FlipechoSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            echoSprite.flipX = true;
            if (echoBody.linearVelocity.x > 0.05f)
                echoAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            echoSprite.flipX = false;
            if (echoBody.linearVelocity.x < -0.05f)
                echoAnimator.SetTrigger("onSkid");
        }
    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (echoBody.linearVelocity.magnitude < maxSpeed)
            echoBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipechoSprite(value);
            moving = true;
            //Debug.Log("MoveCheck() called");
            Move(value);
        }
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            echoBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // Debug.Log("Jumping");
            // update animator state
            echoAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            // Debug.Log("Jumping more");
            echoBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    // GROUND COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // this checks if mario is on the ground
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) && !onGroundState)
        {
            onGroundState = true;
            //update animator state
            echoAnimator.SetBool("onGround", onGroundState);
        }
    }

    // ENEMY COLLISION
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            // detect stomp: Mario is moving downward AND above the enemy (tweak threshold as needed)
            float yThreshold = 0.15f;
            bool movingDown = echoBody != null && echoBody.linearVelocity.y < 0f;
            bool above = transform.position.y > other.transform.position.y + yThreshold;

            if (alive)
            {
                // placeholder first
                Debug.Log("Collided with enemy");
                alive = false;
                Time.timeScale = 0.0f;
            }
        }
    }
    void FallDetector()
    {
        Debug.Log("Echo is falling");
        GameManager.instance.GameOver();
    }

    // POWERUP METHODS
    public void LevelUp()
    {
        Debug.Log("Player LevelUp-ed!");
        echoAudio.PlayOneShot(levelUpClip);
    }

    public void ApplyForm(PowerupType type)
    {
        bool isFire = (type == PowerupType.FireFlower);
        bool IsLevelingUp = type == PowerupType.MagicMushroom;
        echoAnimator.SetBool("IsLevelingUp", IsLevelingUp);
        echoAnimator.SetBool("IsFire", isFire);
    }

    // RESTART CALLBACKS
    public void GameRestart()
    {
        // reset position
        echoBody.transform.position = new Vector3(-8.403f, -3.51f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        echoSprite.flipX = false;
        // reset Mario velocity to 0 to avoid randomly jumping
        echoBody.linearVelocity = Vector3.zero;
        // reset states
        onGroundState = true;
        echoAnimator.SetBool("onGround", true);
        echoAnimator.SetFloat("xSpeed", 0f);
        echoAnimator.SetBool("IsLevelingUp", false);
        echoAnimator.SetBool("IsFire", false);
        // reset score
        //scoreText.text = "Score: 0";
        // reset animation
        echoAnimator.SetTrigger("gameRestart");
        alive = true;
        // reset camera position
        gameCamera.position = new Vector3(-0.9f, -0.5f, -10);
        GameManager.instance.ResetScore();
        // reset powerup state
        powerupState.ResetHighestPowerup();
    }
    
    // ANIMATION AND SOUNDS
    void PlayDeathImpulse()
    {
        echoBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void PlayJumpSound()
    {
        // play jump sound
        echoAudio.PlayOneShot(echoAudio.clip);
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.gameRestart.RemoveListener(GameRestart);
    }
}
