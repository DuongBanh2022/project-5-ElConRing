
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyNPC : MonoBehaviour
{
    private const string DefeatedEnemiesKey = "EnemyDefeatedIDs";
    private static HashSet<string> defeatedEnemyIDs;

    [Header("Enemy Save ID")]
    [SerializeField] private string enemyID = "ForestEnemy_01";

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

        LoadDefeatedEnemies();

        if (!string.IsNullOrWhiteSpace(enemyID) &&
            defeatedEnemyIDs.Contains(enemyID))
        {
            isDead = true;
            Destroy(gameObject);
            return;
        }

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

        if (direction == 0f)
        {
            StopMoving();
            return;
        }

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );

        FaceDirection(direction);
    }

    private void StopMoving()
    {
        if (rb == null)
            return;

        rb.linearVelocity = new Vector2(
            0f,
            rb.linearVelocity.y
        );
    }

    private void FaceTarget(float targetX)
    {
        float direction =
            Mathf.Sign(targetX - transform.position.x);

        if (direction != 0f)
            FaceDirection(direction);
    }

    private void FaceDirection(float direction)
    {
        if (spriteRenderer != null && direction != 0f)
            spriteRenderer.flipX = direction < 0f;
    }

    private void Attack()
    {
        if (player == null || isDead || isAttacking)
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
            Debug.LogWarning("Player does not have PMovement!");
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
        if (isDead)
            return;

        isDead = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        SaveDefeatedEnemy();

        Debug.Log("Enemy defeated and saved: " + enemyID);

        Destroy(gameObject);
    }

    private void LoadDefeatedEnemies()
    {
        if (defeatedEnemyIDs != null)
            return;

        defeatedEnemyIDs = new HashSet<string>();

        string savedIDs =
            PlayerPrefs.GetString(DefeatedEnemiesKey, "");

        if (string.IsNullOrEmpty(savedIDs))
            return;

        foreach (string id in savedIDs.Split('|'))
        {
            if (!string.IsNullOrWhiteSpace(id))
                defeatedEnemyIDs.Add(id);
        }
    }

    private void SaveDefeatedEnemy()
    {
        if (string.IsNullOrWhiteSpace(enemyID))
        {
            Debug.LogWarning(
                "Enemy ID is empty. Enemy defeat was not saved.",
                this
            );
            return;
        }

        LoadDefeatedEnemies();
        defeatedEnemyIDs.Add(enemyID);

        PlayerPrefs.SetString(
            DefeatedEnemiesKey,
            string.Join("|", defeatedEnemyIDs)
        );

        PlayerPrefs.Save();
    }

    // Called by MainMenu when the player chooses New Game.
    public static void ResetAllDefeatedEnemies()
    {
        PlayerPrefs.DeleteKey(DefeatedEnemiesKey);
        PlayerPrefs.Save();

        defeatedEnemyIDs = new HashSet<string>();

        Debug.Log("New Game: all defeated enemies reset.");
    }

    public static void ResetDefeatedEnemy(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        LoadStaticDefeatedEnemies();

        defeatedEnemyIDs.Remove(id);

        PlayerPrefs.SetString(
            DefeatedEnemiesKey,
            string.Join("|", defeatedEnemyIDs)
        );

        PlayerPrefs.Save();
    }

    private static void LoadStaticDefeatedEnemies()
    {
        if (defeatedEnemyIDs != null)
            return;

        defeatedEnemyIDs = new HashSet<string>();

        string savedIDs =
            PlayerPrefs.GetString(DefeatedEnemiesKey, "");

        if (string.IsNullOrEmpty(savedIDs))
            return;

        foreach (string id in savedIDs.Split('|'))
        {
            if (!string.IsNullOrWhiteSpace(id))
                defeatedEnemyIDs.Add(id);
        }
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
