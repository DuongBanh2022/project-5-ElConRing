using Cainos.PixelArtPlatformer_VillageProps;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        // Read movement from the Input System
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        Flip();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
            return;

        if (rb == null)
            return;

        rb.linearVelocity = new Vector2(
            moveInput.x * walkSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f)
            return;

        if (!IsGrounded())
        {
            Debug.Log("NOT GROUNDED");
            return;
        }

        Debug.Log("JUMPING");

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }

    private bool IsGrounded()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (playerCollider == null)
        {
            Debug.LogError("Player needs a Collider2D!");
            return false;
        }

        Bounds bounds = playerCollider.bounds;

        Vector2 checkCenter = new Vector2(
            bounds.center.x,
            bounds.min.y - groundCheckDistance / 2f
        );

        Vector2 checkSize = new Vector2(
            bounds.size.x * 0.8f,
            groundCheckDistance
        );

        Collider2D hit = Physics2D.OverlapBox(
            checkCenter,
            checkSize,
            0f,
            groundLayer
        );

        return hit != null;
    }

    private void Flip()
    {
        if (sr == null)
            return;

        if (moveInput.x < 0)
        {
            sr.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            sr.flipX = false;
        }
    }

    private void UpdateAnimations()
    {
        if (anim == null)
            return;

        bool grounded = IsGrounded();

        bool running =
            Mathf.Abs(moveInput.x) > 0.01f &&
            grounded;

        bool jumping = !grounded;

        anim.SetBool("Running", running);
        anim.SetBool("Jumping", jumping);
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (playerCollider == null)
            return;

        Bounds bounds = playerCollider.bounds;

        Vector2 checkCenter = new Vector2(
            bounds.center.x,
            bounds.min.y - groundCheckDistance / 2f
        );

        Vector2 checkSize = new Vector2(
            bounds.size.x * 0.8f,
            groundCheckDistance
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(
            checkCenter,
            checkSize
        );
    }
}
