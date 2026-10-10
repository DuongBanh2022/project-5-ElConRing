using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyNPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolDistance = 3f;

    [Header("Player Detection")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackAnimationTime = 0.7f;
    [SerializeField] private int attackDamage = 1;

    [Header("Enemy Health")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Transform player;

    private Vector2 startPosition;
    private int patrolDirection = 1;
    private int nextAttackType = 1;

    private bool isAttacking;
    private bool isDead;

    private float nextAttackTime;
    private float attackEndTime;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;

        Debug.Log("Enemy HP: " + currentHealth + "/" + maxHealth);
    }

    private void Update()
    {
        if (isDead || Time.timeScale == 0f)
            return;

        FindPlayer();

        if (player == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= detectionRange)
        {
            FaceTarget(player.position.x);

            if (distance <= attackRange && !isAttacking)
            {
                if (Time.time >= nextAttackTime)
                    Attack();
            }
        }

        if (isAttacking && Time.time >= attackEndTime)
        {
            isAttacking = false;

            if (animator != null)
                animator.SetBool("IsAttacking", false);
        }
    }

    private void FixedUpdate()
    {
        if (isDead || Time.timeScale == 0f)
            return;

        if (isAttacking)
        {
            StopMoving();
            return;
        }

        if (player != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distance <= detectionRange)
            {
                if (distance > attackRange)
                    MoveTowards(player.position.x);
                else
                    StopMoving();

                return;
            }
        }

        Patrol();
    }

    private void FindPlayer()
    {
        if (player != null)
            return;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("Enemy found Player: " + player.name);
        }
    }

    private void Patrol()
    {
        float leftLimit = startPosition.x - patrolDistance;
        float rightLimit = startPosition.x + patrolDistance;

        if (transform.position.x >= rightLimit)
            patrolDirection = -1;
        else if (transform.position.x <= leftLimit)
            patrolDirection = 1;

        rb.linearVelocity = new Vector2(
            patrolDirection * moveSpeed,
            rb.linearVelocity.y
        );

        FaceDirection(patrolDirection);
    }

    private void MoveTowards(float targetX)
    {
        float direction =
            Mathf.Sign(targetX - transform.position.x);

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );

        FaceDirection(direction);
    }

    private void StopMoving()
    {
        rb.linearVelocity = new Vector2(
            0f,
            rb.linearVelocity.y
        );
    }

    private void FaceTarget(float targetX)
    {
        float direction =
            Mathf.Sign(targetX - transform.position.x);

        if (direction != 0)
            FaceDirection(direction);
    }

    private void FaceDirection(float direction)
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = direction < 0;
    }

    private void Attack()
    {
        if (player == null || isDead)
            return;

        isAttacking = true;
        attackEndTime = Time.time + attackAnimationTime;
        nextAttackTime = Time.time + attackCooldown;

        StopMoving();

        Debug.Log("ENEMY ATTACKING PLAYER!");

        if (animator != null)
        {
            animator.SetInteger("AttackType", nextAttackType);
            animator.SetBool("IsAttacking", true);
        }

        PMovement playerMovement =
            player.GetComponent<PMovement>();

        if (playerMovement != null)
        {
            playerMovement.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogWarning(
                "Player does not have PMovement!"
            );
        }

        nextAttackType = nextAttackType == 1 ? 2 : 1;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy TakeDamage called! Damage: " + damage);

        if (isDead)
        {
            Debug.LogWarning("Enemy is already dead!");
            return;
        }

        if (damage <= 0)
        {
            Debug.LogWarning("Damage must be greater than zero!");
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log(
            "ENEMY HP: " + currentHealth + "/" + maxHealth
        );

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        Debug.Log("Enemy defeated!");

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        isAttacking = false;

        if (animator != null)
            animator.SetBool("IsAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}

