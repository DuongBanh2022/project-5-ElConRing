using System.Collections;
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

    [Header("Player Health")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private bool isDead;

    [Header("Starting Point")]
    [SerializeField] private Transform startingPoint;
    [SerializeField] private float respawnDelay = 1f;

    private Vector3 initialPosition;
    private PlayerControls controls;
    private Vector2 moveInput;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        controls = new PlayerControls();
        currentHealth = maxHealth;

        if (startingPoint != null)
            initialPosition = startingPoint.position;
        else
            initialPosition = transform.position;
    }

    private void OnEnable()
    {
        if (controls == null)
            controls = new PlayerControls();

        controls.Player.Jump.performed += OnJump;
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Jump.performed -= OnJump;
            controls.Player.Disable();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        Debug.Log("PLAYER HP: " + currentHealth + "/" + maxHealth);
    }

    private void Update()
    {
        if (Time.timeScale == 0f || isDead)
            return;

        // Read movement from the Input System
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        Flip();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f || isDead)
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
        if (Time.timeScale == 0f || isDead)
            return;

        if (!IsGrounded())
        {
            Debug.Log("NOT GROUNDED");
            return;
        }

        Debug.Log("JUMPING");

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player TakeDamage called! Damage: " + damage);

        if (isDead)
        {
            Debug.LogWarning("Player is already dead!");
            return;
        }

        if (damage <= 0)
        {
            Debug.LogWarning("Damage must be greater than zero!");
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log("PLAYER HP: " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead || amount <= 0)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        Debug.Log("PLAYER HEALED: " + currentHealth + "/" + maxHealth);
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        moveInput = Vector2.zero;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (anim != null)
        {
            anim.SetBool("Running", false);
            anim.SetBool("Jumping", false);
        }

        Debug.Log("PLAYER DIED! Respawning soon...");

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSecondsRealtime(respawnDelay);

        Vector3 respawnPosition = startingPoint != null
            ? startingPoint.position
            : initialPosition;

        transform.position = respawnPosition;

        if (rb != null)
        {
            rb.position = new Vector2(
                respawnPosition.x,
                respawnPosition.y
            );

            rb.linearVelocity = Vector2.zero;
        }

        Physics2D.SyncTransforms();

        currentHealth = maxHealth;
        moveInput = Vector2.zero;
        isDead = false;

        if (anim != null)
        {
            anim.SetBool("Running", false);
            anim.SetBool("Jumping", false);
        }

        Debug.Log("PLAYER RESPAWNED at " + respawnPosition);
        Debug.Log("PLAYER HP: " + currentHealth + "/" + maxHealth);
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