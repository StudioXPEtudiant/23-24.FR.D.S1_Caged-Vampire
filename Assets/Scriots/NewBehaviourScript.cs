using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private int jumpCount = 0;
    public int maxJumpCount = 2; // Set the maximum number of jumps here.
    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for jump input using the 'W' key.
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;
    }

    void Jump()
    {
        if (isGrounded || jumpCount < maxJumpCount)
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity before the jump.
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    // Detect collisions with the ground.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // Reset jump count when grounded.
        }
    }

    // Detect when the player leaves the ground.
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

