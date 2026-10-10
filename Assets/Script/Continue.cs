using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        // Check if a saved checkpoint exists.
        bool hasSave = PlayerPrefs.GetInt("HasLastLocation", 0) == 1;

        continueButton.SetActive(hasSave);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("HasLastLocation", 0) == 1)
        {
            Debug.Log("Saved checkpoint found. Continuing game.");

            SceneManager.LoadScene("ContinueScene");
        }
        else
        {
            Debug.Log("No saved checkpoint found.");
        }
    }
}
