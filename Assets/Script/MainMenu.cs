using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Reset defeated enemies for a fresh game.
        EnemyNPC.ResetAllDefeatedEnemies();

        // Clear previous checkpoint data.
        PlayerPrefs.DeleteKey("LastX");
        PlayerPrefs.DeleteKey("LastY");
        PlayerPrefs.DeleteKey("LastZ");
        PlayerPrefs.DeleteKey("HasLastLocation");
        PlayerPrefs.DeleteKey("A4Checkpoint");

        PlayerPrefs.Save();

        // Start the loading scene.
        SceneManager.LoadScene("LoadingScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
