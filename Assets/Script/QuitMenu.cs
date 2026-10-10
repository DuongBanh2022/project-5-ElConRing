using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 position = player.transform.position;

            // Save the player's last location.
            PlayerPrefs.SetFloat("LastX", position.x);
            PlayerPrefs.SetFloat("LastY", position.y);
            PlayerPrefs.SetFloat("LastZ", position.z);

            PlayerPrefs.SetInt("HasLastLocation", 1);

            // Save PlayerPrefs to disk.
            PlayerPrefs.Save();

            Debug.Log("LAST LOCATION SAVED: " + position);
        }
        else
        {
            Debug.LogWarning(
                "Player not found. Make sure the Player has the Player tag."
            );
        }

        // Reset pause state before changing scenes.
        Time.timeScale = 1f;

        // Return to Main Menu.
        SceneManager.LoadScene("ExitScene");
    }
}


