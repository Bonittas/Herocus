using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float jumpForce = 5f;
    public float fallSpeed = 5f;          // Speed for falling
    private bool isGrounded = true;        // Whether the character is grounded
    private bool isFacingRight = true;     // Track direction

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MovementControls();
        ActionControls();
    }

    // Movement Controls for Walking, Running, Jumping, Turning
    void MovementControls()
    {
        if (isGrounded)
        {
            float moveDirection = 0f;

            if (Input.GetKey(KeyCode.Space)) // Walking
            {
                moveDirection = isFacingRight ? 1f : -1f;
                MoveCharacter(walkSpeed, moveDirection);
                SetWalking(true);
            }
            else
            {
                SetWalking(false);
            }

            if (Input.GetKey(KeyCode.R)) // Running
            {
                moveDirection = isFacingRight ? 1f : -1f;
                MoveCharacter(runSpeed, moveDirection);
                SetRunning(true);
            }
            else
            {
                SetRunning(false);
            }

            if (Input.GetKeyDown(KeyCode.J)) // Jumping
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.L)) // Turn left
            {
                TurnLeft();
            }

            if (Input.GetKeyDown(KeyCode.K)) // Turn right
            {
                TurnRight();
            }
        }
    }

    // Controls for Actions like Attacking, Defeat, Taking Damage
    void ActionControls()
    {
        if (Input.GetKeyDown(KeyCode.A)) // Attacking
        {
            SetAttacking(true);
        }

        if (Input.GetKeyDown(KeyCode.D)) // Defeat
        {
            SetDefeat(true);
        }

        if (Input.GetKeyDown(KeyCode.E)) // Taking damage
        {
            SetDamage(true);
        }
    }

    // Move the character horizontally
    void MoveCharacter(float speed, float direction)
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    // Walking logic with boolean
    void SetWalking(bool state)
    {
        animator.SetBool("isWalking", state);
    }

    // Running logic with boolean
    void SetRunning(bool state)
    {
        animator.SetBool("isRunning", state);
    }

    // Jump logic with boolean
    void Jump()
    {
        if (isGrounded) // Only allow jump if grounded
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
            StartCoroutine(EndJump());
        }
    }

    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(0.5f); // Adjust to match the jump animation duration
        animator.SetBool("isJumping", false);
    }

    // Attack logic with boolean
    void SetAttacking(bool state)
    {
        animator.SetBool("isAttacking", state);
        if (state) StartCoroutine(EndAttack());
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.5f); // Adjust to match attack animation duration
        animator.SetBool("isAttacking", false);
    }

    // Defeat logic with boolean
    void SetDefeat(bool state)
    {
        animator.SetBool("isDefeated", state);
        if (state) StartCoroutine(EndDefeat());
    }

    IEnumerator EndDefeat()
    {
        yield return new WaitForSeconds(1f); // Adjust to match defeat animation duration
        animator.SetBool("isDefeated", false);
    }

    // Damage logic with boolean
    void SetDamage(bool state)
    {
        animator.SetBool("isDamaged", state);
        if (state) StartCoroutine(EndDamage());
    }

    IEnumerator EndDamage()
    {
        yield return new WaitForSeconds(0.5f); // Adjust to match damage animation duration
        animator.SetBool("isDamaged", false);
    }

    // Flip the character to face left or right
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Invert the x scale to flip direction
        transform.localScale = scale;
    }

    // Turn left (Flip if currently facing right)
    void TurnLeft()
    {
        if (isFacingRight) // Only flip if facing right
        {
            Flip();
        }
    }

    // Turn right (Flip if currently facing left)
    void TurnRight()
    {
        if (!isFacingRight) // Only flip if facing left
        {
            Flip();
        }
    }

    // Ground detection using colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if character is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Character is on the ground
            animator.SetBool("isGrounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // When character leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Character is no longer on the ground
            Fall();
        }
    }

    // Falling logic with boolean
    void Fall()
    {
        rb.velocity = new Vector2(rb.velocity.x, -fallSpeed); // Apply falling force
        animator.SetBool("isFalling", true);
        StartCoroutine(EndFall());
    }

    IEnumerator EndFall()
    {
        yield return new WaitForSeconds(0.5f); // Adjust to match falling animation duration
        animator.SetBool("isFalling", false);
    }
}
