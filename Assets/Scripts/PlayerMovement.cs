using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float normalMoveSpeed = 5f;
    [SerializeField] private float crouchMoveSpeed = 2f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float crouchJumpForce = 2f;
    [SerializeField] private float dashJumpForce = 7f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCooldown = 2f;

    [Header("Camera distance")]
    [SerializeField] private float normalCameraDistance = 5f;
    [SerializeField] private float crouchCameraDistance = 3.5f;

    private float _dashCooldownTimer;
    private int _remainingJumps = 2;

    
    private bool _isGrounded;
    private bool _isCrouching;
    private bool _isDashing;

    private Rigidbody2D _rigidbody2D;
    private CameraManager _camera;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _camera = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
    }

    private void Update()
    {
        _dashCooldownTimer += Time.deltaTime;

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && _dashCooldownTimer >= dashCooldown)
            Dash();

        if (Input.GetKeyDown(KeyCode.W))
            Jump();

        if (Input.GetKeyDown(KeyCode.S))
            Crouch();

        float movementInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(movementInput, 0f, 0f);
        _animator.SetFloat("Move", Mathf.Abs(movementInput));

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

        if (movementInput < 0f)
            _spriteRenderer.flipX = false;
        else if (movementInput > 0f)
            _spriteRenderer.flipX = true;
    }
    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Ground"));

        _isGrounded = colliders.Length == 1; // Check if any colliders are detected

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collider detected: " + collider.gameObject.name);
        }

        Debug.Log("Is Grounded: " + _isGrounded);

        if (_isGrounded)
        {
            _remainingJumps = 2;
            Debug.Log("Jumps Reset to 2");
        }
    }




    private void Jump()
    {
        if (_remainingJumps > 0)
        {
            float jumpForceToApply = _isCrouching ? crouchJumpForce : jumpForce;

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.AddForce(new Vector2(0f, jumpForceToApply), ForceMode2D.Impulse);

            _remainingJumps--;
            _animator.SetTrigger("Jumping");
        }
    }

    private void Crouch()
    {
        _isCrouching = !_isCrouching;
        if (!_isCrouching)
        {
            _animator.SetTrigger("Uncrouching");
        }
        else
        {
            _animator.SetTrigger("Crouching");
        }
    }

    private void Dash()
    {
        if (!_isDashing && _dashCooldownTimer >= dashCooldown)
        {
            float dashDirection = Mathf.Sign(Input.GetAxis("Horizontal"));
            StartCoroutine(PerformDash(dashDirection));
            _animator.SetTrigger("Dashing");
        }
    }

    private IEnumerator PerformDash(float dashDirection)
    {
        _isDashing = true;
        _rigidbody2D.velocity = new Vector2(dashSpeed * dashDirection, _rigidbody2D.velocity.y);

        yield return new WaitForSeconds(0.5f); // Adjust dash duration as needed

        _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
        _dashCooldownTimer = 0f;
        _isDashing = false;
    }
}
