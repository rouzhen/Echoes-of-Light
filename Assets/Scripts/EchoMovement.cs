using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class EchoMovement : MonoBehaviour
{
    /* Variable declarations */
    public float speed = 10;
    private bool onGroundState = true;
    private Rigidbody2D echoBody;

    public float maxSpeed = 20;
    public float upSpeed = 10;
    private SpriteRenderer echoSprite;
    private bool faceRightState = true;
    public TextMeshProUGUI scoreText;

    public GameObject gameManager;

    public TextMeshProUGUI finalScore;
    public Animator echoAnimator;
    public AudioSource echoAudio;
    public AudioSource echoDeath;
    public float deathImpulse = 15;
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    // state
    [System.NonSerialized]
    public bool alive = true;
    public static System.Action OnGameRestart;
    public Transform gameCamera;
    private bool moving = false;
    private bool jumpedState = false;



    /*** Unity Callbacks ***/

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        echoBody = GetComponent<Rigidbody2D>();
        echoSprite = GetComponent<SpriteRenderer>();
        // update animator state
        echoAnimator.SetBool("onGround", onGroundState);



    }

    // Update is called once per frame
    void Update()
    {
        echoAnimator.SetFloat("xSpeed", Mathf.Abs(echoBody.linearVelocity.x));
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    /*** Movement Control ***/
    void FlipechoSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            echoSprite.flipX = true;
            /*if (echoBody.linearVelocity.x > 0.05f)
                echoAnimator.SetTrigger("onSkid");*/

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            echoSprite.flipX = false;
            /*if (echoBody.linearVelocity.x < -0.05f)
                echoAnimator.SetTrigger("onSkid");*/
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
            Debug.Log("MoveCheck() called");
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


    /*** OnCollisions ***/

    // Mario collides with ground
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

    // Mario collides with Goomba
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            // detect stomp: Mario is moving downward AND above the enemy (tweak threshold as needed)
            float yThreshold = 0.15f;
            bool movingDown = echoBody != null && echoBody.linearVelocity.y < 0f;
            bool above = transform.position.y > other.transform.position.y + yThreshold;

        }
    }



    /*** Game Restart ***/
    public void RestartButtonCallback(int input)
    {
        // reset everything
        GameRestart();
        // resume time
        Time.timeScale = 1.0f;
    }


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

        // reset score
        scoreText.text = "Score: 0";


        // reset animation
        echoAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(-0.9f, -0.5f, -10);

    }

    /*** Animation and Sounds ***/
    void PlayDeathImpulse()
    {
        echoBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void PlayJumpSound()
    {
        // play jump sound
        echoAudio.PlayOneShot(echoAudio.clip);
    }
}
