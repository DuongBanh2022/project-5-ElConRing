using UnityEngine;

public class EnemyNPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 3f;

    [Header("Health")]
    [SerializeField] private int maxHP = 3;
    [SerializeField] private EnemyHealthBar healthBar;

    private int currentHP;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    private bool movingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(
                "EnemyNPC needs a Rigidbody2D on the Enemy!"
            );
        }
    }

    private void Start()
    {
        startPosition = transform.position;

        currentHP = maxHP;

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHP, maxHP);
        }

        Debug.Log(
            "Enemy spawned with HP: " +
            currentHP +
            "/" +
            maxHP
        );
    }

    private void FixedUpdate()
    {
        // Stop enemy while the game is paused.
        if (Time.timeScale == 0f)
            return;

        if (rb == null)
            return;

        // Calculate how far the enemy has moved
        // from its original position.
        float distanceFromStart =
            rb.position.x - startPosition.x;

        // Change direction at the right boundary.
        if (distanceFromStart >= moveDistance)
        {
            movingRight = false;
        }

        // Change direction at the left boundary.
        else if (distanceFromStart <= -moveDistance)
        {
            movingRight = true;
        }

        float direction = movingRight ? 1f : -1f;

        // Move using Rigidbody2D physics.
        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log(
            "Enemy HP: " +
            currentHP +
            "/" +
            maxHP
        );

        // Update the HP bar.
        if (healthBar != null)
        {
            healthBar.UpdateHealth(
                currentHP,
                maxHP
            );
        }

        // Enemy dies at 0 HP.
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("ENEMY DEFEATED!");

        // Disable the enemy instead of destroying it.
        // This is safer for the pooled/project setup.
        gameObject.SetActive(false);
    }
}




