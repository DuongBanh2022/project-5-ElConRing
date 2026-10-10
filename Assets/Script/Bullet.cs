using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    public float speed = 8f;
    public float lifetime = 2f;

    private SimplePool pool;
    private float timer;

    public void SetPool(SimplePool bulletPool)
    {
        pool = bulletPool;
    }

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        transform.Translate(
            Vector3.right * speed * Time.deltaTime,
            Space.Self
        );

        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyNPC enemy = other.GetComponent<EnemyNPC>();

        if (enemy == null)
            enemy = other.GetComponentInParent<EnemyNPC>();

        if (enemy != null)
        {
            Debug.Log("BULLET HIT ENEMY!");

            enemy.TakeDamage(1);

            // Return the bullet to your pool here
            // if your project uses object pooling.
        }
    


    // Breakable object damage
        WallHP breakable =
                other.GetComponent<WallHP>();

        if (breakable != null)
        {
            Debug.Log("BULLET HIT BREAKABLE OBJECT!");

            breakable.TakeDamage(damage);

            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (pool != null)
        {
            pool.ReturnToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

