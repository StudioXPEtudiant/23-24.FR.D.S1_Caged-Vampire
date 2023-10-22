using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float normalMoveSpeed = 5f; // Normal walking speed.
    [SerializeField] private float crouchMoveSpeed = 2f; // Slower walking speed while crouching.

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f; // Normal jump force.
    [SerializeField] private float crouchJumpForce = 2f; // Lower jump force when crouching.
    [SerializeField] private float dashJumpForce = 7f; // Jump force after dashing in the air.

    [Header("Camera distance")]
    [SerializeField] private float normalCameraDistance = 5f; // Normal camera distance.
    [SerializeField] private float crouchCameraDistance = 3.5f; // Closer camera distance while crouching.

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10f; // Speed of the dash
    [SerializeField] private float dashCooldown = 2f; // Cooldown time before the next dash

    private bool _isGrounded;
    private bool _isCrouching;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    private int remainingJumps = 1; // The player starts with 1 jump.

    private Rigidbody2D _rigidbody2D;
    private CameraManager _camera;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Jump();
        if (Input.GetKeyDown(KeyCode.S)) Crouch();

        // Check if the Shift key is pressed for dashing and if the cooldown has expired
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && dashCooldownTimer <= 0f)
        {
            Dash();
        }

        // Update the dash cooldown timer
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        var movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        if (_isCrouching)
        {
            _camera.ChangeFieldView(crouchCameraDistance, 2f);
            movement *= crouchMoveSpeed;
        }
        else
        {
            _camera.ChangeFieldView(normalCameraDistance, 5f);
            movement *= normalMoveSpeed;
        }

        transform.position += movement * Time.deltaTime;
    }

    private void Jump()
    {
        float jumpForceToApply;

        if (_isGrounded || remainingJumps > 0)
        {
            if (isDashing && !_isGrounded)
            {
                jumpForceToApply = dashJumpForce;
                remainingJumps = 0; // Reset jumps after dashing in the air.
            }
            else
            {
                jumpForceToApply = _isCrouching ? crouchJumpForce : jumpForce;
            }

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.AddForce(new Vector2(0f, jumpForceToApply), ForceMode2D.Impulse);
        }
    }

    private void Crouch()
    {
        _isCrouching = !_isCrouching;
        transform.localScale = _isCrouching ? new Vector3(1f, 0.5f, 1f) : Vector3.one;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            remainingJumps = 1; // Reset jumps when landing.
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            // Determine the dash direction based on the player's horizontal input
            float dashDirection = Input.GetAxis("Horizontal");

            StartCoroutine(PerformDash(dashDirection));
        }
    }

    private IEnumerator PerformDash(float dashDirection)
    {
        isDashing = true;

        // Set the velocity to dashSpeed in the direction determined by dashDirection
        _rigidbody2D.velocity = new Vector2(dashSpeed * dashDirection, _rigidbody2D.velocity.y);

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Reset the velocity to zero to stop the dash
        _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);

        // Start the dash cooldown
        dashCooldownTimer = dashCooldown;
        isDashing = false;
    }
}
