using UnityEngine;

public class SaveCheckpoint : MonoBehaviour
{
    private void Start()
    {
        LoadCheckpoint();
    }

    public void SaveCurrentCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning(
                "Player not found. Make sure the Player has the Player tag."
            );

            return;
        }

        Vector3 position = player.transform.position;

        // Save the player's location.
        PlayerPrefs.SetFloat("LastX", position.x);
        PlayerPrefs.SetFloat("LastY", position.y);
        PlayerPrefs.SetFloat("LastZ", position.z);

        // Remember that a checkpoint exists.
        PlayerPrefs.SetInt("HasLastLocation", 1);

        // Save PlayerPrefs to disk.
        PlayerPrefs.Save();

        Debug.Log("CHECKPOINT SAVED: " + position);
    }

    private void LoadCheckpoint()
    {
        if (PlayerPrefs.GetInt("HasLastLocation", 0) != 1)
        {
            Debug.Log("No saved checkpoint found.");
            return;
        }

        float x = PlayerPrefs.GetFloat("LastX", 0f);
        float y = PlayerPrefs.GetFloat("LastY", 0f);
        float z = PlayerPrefs.GetFloat("LastZ", 0f);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning(
                "Player not found. Make sure the Player has the Player tag."
            );

            return;
        }

        player.transform.position = new Vector3(x, y, z);

        Debug.Log(
            "CHECKPOINT LOADED: " + player.transform.position
        );
    }
}


