using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI text

namespace ClearSky
{
    public class DemoCollegeStudentController : MonoBehaviour
    {
        public float movePower = 10f;
        public float KickBoardMovePower = 15f;
        public float jumpPower = 8f;
        public bool autoRun = true;

        private Rigidbody2D rb;
        private Animator anim;
        private PlayerHealth playerHealth;
        private int direction = 1;
        private bool isJumping = false;
        private bool alive = true;
        private bool isKickboard = false;
        private int totalCoins = 0; // To keep track of collected coins

        // Sound effects
        public AudioClip coinSound; // Add the coin sound clip
        public AudioClip runSound;  // Add the run sound clip
        private AudioSource audioSource; // AudioSource to play sounds

        // UI Elements
        public Text coinCounterText; // Reference to the UI Text element

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            playerHealth = GetComponent<PlayerHealth>();

            audioSource = GetComponent<AudioSource>(); // Initialize AudioSource

            if (audioSource == null)
            {
                Debug.LogError("No AudioSource attached to the player. Please attach one.");
            }

            if (autoRun)
            {
                direction = 1;
            }

            // Initialize coin counter display
            UpdateCoinCounterText();
        }

        private void Update()
        {
            if (playerHealth.GetCurrentHealth() > 0 && alive)
            {
                Restart();
                Hurt();
                Die();
                Attack();
                Jump();
                KickBoard();
                Run();  // Ensure Run is being called in Update
            }
        }

        // When player touches a coin
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Coin")) // Make sure the coin prefab has the tag "Coin"
            {
                totalCoins++; // Increase the coin counter
                Destroy(other.gameObject); // Make the coin disappear
                Debug.Log("Coins collected: " + totalCoins);

                // Update coin counter display
                UpdateCoinCounterText();

                // Play coin collection sound
                PlaySound(coinSound);
            }

            anim.SetBool("isJump", false);
        }

        void KickBoard()
        {
            if (Input.GetKeyDown(KeyCode.Alpha4) && isKickboard)
            {
                isKickboard = false;
                anim.SetBool("isKickBoard", false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && !isKickboard)
            {
                isKickboard = true;
                anim.SetBool("isKickBoard", true);
            }
        }

        void Run()
        {
            if (!isKickboard)
            {
                Vector3 moveVelocity = Vector3.zero;
                anim.SetBool("isRun", false);

                // Handle automatic running
                if (autoRun)
                {
                    moveVelocity = Vector3.right * direction; // Move automatically
                }
                else
                {
                    // Handle player input
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        direction = -1;
                        moveVelocity = Vector3.left;
                    }
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        direction = 1;
                        moveVelocity = Vector3.right;
                    }
                }

                transform.localScale = new Vector3(direction, 1, 1);

                // Check if the player is running (not jumping)
                if (!anim.GetBool("isJump"))
                {
                    anim.SetBool("isRun", true);

                    // Play run sound if not already playing
                    if (!audioSource.isPlaying)
                    {
                        PlaySound(runSound, true); // Loop run sound while running
                    }
                }
                else
                {
                    // Stop run sound when jumping
                    audioSource.Stop();
                }

                // Apply movement
                transform.position += moveVelocity * movePower * Time.deltaTime;
            }

            if (isKickboard)
            {
                Vector3 moveVelocity = Vector3.zero;
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    direction = -1;
                    moveVelocity = Vector3.left;
                }
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    direction = 1;
                    moveVelocity = Vector3.right;
                }
                transform.position += moveVelocity * KickBoardMovePower * Time.deltaTime;

                // Stop running sound when on the kickboard
                audioSource.Stop();
            }
        }

        void Jump()
        {
            if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0) && !anim.GetBool("isJump"))
            {
                isJumping = true;
                anim.SetBool("isJump", true);
            }
    
            if (isJumping)
            {
                // Adjust gravity scale for a more controlled jump and fall
                rb.gravityScale = 3f; // Increase this value if the fall is still too slow

                // Clear previous velocity
                rb.velocity = new Vector2(rb.velocity.x, 0);

                // Apply jump force
                rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        
                isJumping = false;
            }
        }

        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("attack");
            }
        }

        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }

        void Die()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                isKickboard = false;
                anim.SetBool("isKickBoard", false);
                anim.SetTrigger("die");
                alive = false;
            }
        }

        void Restart()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                isKickboard = false;
                anim.SetBool("isKickBoard", false);
                anim.SetTrigger("idle");
                alive = true;
            }
        }

        // Helper method to play sounds
        private void PlaySound(AudioClip clip, bool loop = false)
        {
            if (clip != null && audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.loop = loop;
                audioSource.Play();
            }
        }

        // Method to update the coin counter text
        private void UpdateCoinCounterText()
        {
            if (coinCounterText != null)
            {
                coinCounterText.text = "Coins: " + totalCoins;
            }
            else
            {
                Debug.LogWarning("CoinCounterText is not assigned.");
            }
        }
    }
}
