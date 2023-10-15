using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Speed")]
    [SerializeField] private float normalMoveSpeed = 5f; // Normal walking speed.
    [SerializeField] private float crouchMoveSpeed = 2f; // Slower walking speed while crouching.
    
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f; // Normal jump force.
    [SerializeField] private float crouchJumpForce = 2f; // Lower jump force when crouching.
    
    [Header("Camera distance")]
    [SerializeField] private float normalCameraDistance = 5f; // Normal camera distance.
    [SerializeField] private float crouchCameraDistance = 3.5f; // Closer camera distance while crouching.
    
    private bool _isGrounded;
    private bool _isCrouching;

    private Rigidbody2D _rigidbody2D;
    private CameraManager _camera;

    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Jump();
        if (Input.GetKeyDown(KeyCode.S)) Crouch();
        
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
        if (!_isGrounded) return;
        var jumpForceToApply = _isCrouching ? crouchJumpForce : jumpForce;
        
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
        _rigidbody2D.AddForce(new Vector2(0f, jumpForceToApply), ForceMode2D.Impulse);
    }

    private void Crouch() {
        _isCrouching = !_isCrouching;
        transform.localScale = _isCrouching ? new Vector3(1f, 0.5f, 1f) : Vector3.one;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            _isGrounded = false;
        }
    }
}
