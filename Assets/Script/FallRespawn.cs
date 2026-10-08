using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    [Header("Default Respawn")]
    [SerializeField] private float defaultX = -120f;
    [SerializeField] private float defaultY = 0f;
    [SerializeField] private float defaultZ = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("PLAYER FELL! RESPAWNING...");

        Vector3 respawnPosition;

        // Use saved checkpoint if one exists
        if (PlayerPrefs.GetInt("HasLastLocation", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat("LastX");
            float y = PlayerPrefs.GetFloat("LastY");
            float z = PlayerPrefs.GetFloat("LastZ");

            respawnPosition = new Vector3(x, y, z);

            Debug.Log("Using saved checkpoint.");
        }
        else
        {
            // Default respawn position
            respawnPosition = new Vector3(
                defaultX,
                defaultY,
                defaultZ
            );

            Debug.Log("No save found. Using default respawn.");
        }

        other.transform.position = respawnPosition;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log(
            "PLAYER RESPAWNED AT: " +
            respawnPosition
        );
    }
}






