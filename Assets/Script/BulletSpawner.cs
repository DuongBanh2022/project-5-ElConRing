using UnityEngine;
using UnityEngine.InputSystem;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Pool")]
    public SimplePool Pool;

    [Header("Bullet Settings")]
    public float spawnDistance = 1f;

    private SpriteRenderer playerSprite;
    private Animator playerAnimator;

    private bool useFirstSwing = true;

    private void Start()
    {
        playerSprite =
            GetComponentInParent<SpriteRenderer>();

        playerAnimator =
            GetComponentInParent<Animator>();

        if (playerSprite == null)
        {
            Debug.LogError(
                "BulletSpawner: Player SpriteRenderer not found!"
            );
        }

        if (playerAnimator == null)
        {
            Debug.LogError(
                "BulletSpawner: Player Animator not found!"
            );
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Fire();
        }
    }

    public void Fire()
    {
        if (Pool == null)
        {
            Debug.LogError(
                "BulletSpawner: Pool is not assigned!"
            );

            return;
        }

        GameObject bullet = Pool.GetFromPool();

        if (bullet == null)
        {
            Debug.LogWarning(
                "No bullet available in pool!"
            );

            return;
        }

        if (playerSprite == null)
        {
            Debug.LogError(
                "BulletSpawner: Player SpriteRenderer not found!"
            );

            Pool.ReturnToPool(bullet);
            return;
        }

        // Alternate between the two swing animations.
        if (playerAnimator != null)
        {
            if (useFirstSwing)
            {
                playerAnimator.SetTrigger("Swing1");

                Debug.Log("PLAYER SWING 1");
            }
            else
            {
                playerAnimator.SetTrigger("Swing2");

                Debug.Log("PLAYER SWING 2");
            }

            useFirstSwing = !useFirstSwing;
        }

        bool facingLeft = playerSprite.flipX;

        float direction =
            facingLeft ? -1f : 1f;

        Vector3 spawnPosition =
            transform.position +
            Vector3.right *
            direction *
            spawnDistance;

        bullet.transform.position =
            spawnPosition;

        bullet.transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                facingLeft ? 180f : 0f
            );

        Bullet Bullet =
            bullet.GetComponent<Bullet>();

        if (Bullet != null)
        {
           Bullet.SetPool(Pool);
        }
        else
        {
            Debug.LogError(
                "Bullet prefab is missing " +
                "PooledBullet component!"
            );
        }

        if (facingLeft)
        {
            Debug.Log("BULLET FIRED LEFT");
        }
        else
        {
            Debug.Log("BULLET FIRED RIGHT");
        }
    }
}








