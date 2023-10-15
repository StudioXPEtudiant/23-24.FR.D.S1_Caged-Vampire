using UnityEngine;

public class walkfunction : MonoBehaviour
{
    public float normalMoveSpeed = 5f; // Normal walking speed.
    public float crouchMoveSpeed = 2f; // Slower walking speed while crouching.
    public float jumpForce = 5f; // Normal jump force.
    public float crouchJumpForce = 2f; // Lower jump force when crouching.
    public float normalCameraDistance = 5f; // Normal camera distance.
    public float crouchCameraDistance = 3.5f; // Closer camera distance while crouching.
    private int jumpCount = 0;
    public int maxJumpCount = 2;
    private bool isGrounded = false;
    private bool isCrouching = false;

    private Camera mainCamera; // Reference to the main camera.

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera.
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleCrouch();
        }

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        if (isCrouching)
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, crouchCameraDistance, Time.deltaTime * 2f);
            movement *= crouchMoveSpeed;
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, normalCameraDistance, Time.deltaTime * 5f);
            movement *= normalMoveSpeed;
        }

        transform.position += movement * Time.deltaTime;
    }

    void Jump()
    {
        if (isGrounded || jumpCount < maxJumpCount)
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            float jumpForceToApply = isCrouching ? crouchJumpForce : jumpForce;
            rb.AddForce(new Vector2(0f, jumpForceToApply), ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

