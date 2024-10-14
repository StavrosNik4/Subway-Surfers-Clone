using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference to the Rigidbody component, which controls physics-based movement
    private Rigidbody playerRb;

    // Reference to the Animator component, which controls animations for the player
    private Animator playerAnim;

    // Reference to the AudioSource component on the player, used for playing sounds
    private AudioSource playerAudio;

    // Reference to the AudioSource component on the Main Camera, used for background music
    private AudioSource cameraAudio;

    private GameManager gameManager;




    // Force applied to the player when they jump
    public float jumpForce = 8.0f;

    // Modifier for gravity, used to change the gravity affecting the player
    public float gravityModifier = 1.0f;

    // Flag to check if the player is currently on the ground
    public bool isOnGround = true;

    public bool isNotSliding = true;

    // Flag to check if the game is over
    public bool gameOver = false;

    // References to the ParticleSystem components for explosion and dirt effects
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    // References to the AudioClip components for jump and crash sounds
    public AudioClip jumpSound;
    public AudioClip slideSound;
    public AudioClip crashSound;
    public AudioClip sipSound;


    // Reference to the BoxCollider component
    private BoxCollider boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Rigidbody component by getting it from the GameObject this script is attached to
        playerRb = GetComponent<Rigidbody>();

        // Initialize the Animator component by getting it from the GameObject this script is attached to
        playerAnim = GetComponent<Animator>();

        // Initialize the AudioSource component by getting it from the GameObject this script is attached to
        playerAudio = GetComponent<AudioSource>();

        // Get the AudioSource component from the Main Camera, used to control background music
        cameraAudio = Camera.main.GetComponent<AudioSource>();

        // Initialize the Game Manager component
        gameManager = GameObject.Find("EmptyGameObject").GetComponent<GameManager>();

        // If the camera doesn't have an AudioSource, log an error
        if (cameraAudio == null)
        {
            Debug.LogError("No AudioSource found on the Main Camera.");
        }

        // Modify the global gravity by multiplying it with the gravityModifier
        Physics.gravity *= gravityModifier;

        // Increase the player's mass slightly to alter its physics behavior
        playerRb.mass *= 1.1f;


        boxCollider = GetComponent<BoxCollider>();

        // Check if the BoxCollider exists
        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider component found on this GameObject.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the spacebar, is on the ground, and the game is not over
        if (Input.GetKeyDown(KeyCode.UpArrow) && isOnGround && isNotSliding && !gameOver)
        {
            // Play the jump sound using the player's AudioSource
            playerAudio.PlayOneShot(jumpSound, 1.0f);

            // Stop the dirt particle effect when jumping
            dirtParticle.Stop();

            // Apply an upward force to the player to make them jump
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Set the flag to false since the player is no longer on the ground
            isOnGround = false;

            // Trigger the jump animation
            playerAnim.SetTrigger("Jump_trig");

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && isOnGround && isNotSliding && !gameOver)
        {
            // down key handle

            playerAudio.PlayOneShot(slideSound, 1.0f);
            playerAnim.SetTrigger("Slide_trig");

            isNotSliding = false;

            SetColliderSize(new Vector3(1.0f, 2.0f, 1.0f));

            // Reset collider size after some time (for example, 1 second)
            Invoke("ResetColliderSize", 0.8f);

        }
    }

    // This method is called when the player collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player has collided with the ground
        if (collision.gameObject.CompareTag("Ground") && gameOver == false)
        {
            // Set the flag to true since the player is on the ground
            isOnGround = true;

            // Play the dirt particle effect when the player lands
            dirtParticle.Play();
        }
        // Check if the player has collided with an obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {

            // Stop the background music when the player crashes
            cameraAudio.Stop();

            // Play the crash sound using the player's AudioSource
            playerAudio.PlayOneShot(crashSound, 1.0f);

            // Play the explosion particle effect when the player crashes
            explosionParticle.Play();

            // Set the game over flag to true
            gameOver = true;

            // Log "Game Over!" to the console
            Debug.Log("Game Over!");

            // Trigger the death animation
            playerAnim.SetBool("Death_b", true);

            // Set the type of death animation to play
            playerAnim.SetInteger("DeathType_int", 1);

            // Stop the dirt particle effect when the game is over
            dirtParticle.Stop();

            gameManager.GameOver(false);
        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            gameManager.UpdateScore(1);

            playerAudio.PlayOneShot(sipSound, 1.0f);

            if (gameManager.getScore() == 13) {

                // Stop the background music when the player crashes
                cameraAudio.Stop();

                // Set the game over flag to true
                gameOver = true;

                // Log "Game Over!" to the console
                Debug.Log("Game Over!");

                // Stop the dirt particle effect when the game is over
                dirtParticle.Stop();

                playerAnim.ResetTrigger("Jump_trig");

                playerAnim.ResetTrigger("Slide_trig");

                playerAnim.Play("Idle");

                playerAnim.speed = 0;

                gameManager.GameOver(true);
            }

            Destroy(collision.gameObject);
        }   
    }


    // Method to set the size of the BoxCollider
    public void SetColliderSize(Vector3 newSize)
    {
        if (boxCollider != null)
        {
            boxCollider.size = newSize;
        }
    }

    private void ResetColliderSize()
    {

        isNotSliding = true;

        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(1.0f, 3.0f, 1.0f);
        }
    }
}
