using UnityEngine;

public class DiamondCollectible : MonoBehaviour
{
    [Header("Completion Panel")]
    [SerializeField] private GameObject completionPanel;

    private bool collected = false;

    private void Start()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (other.CompareTag("Player"))
        {
            CollectDiamond();
        }
    }

    private void CollectDiamond()
    {
        collected = true;

        Debug.Log("DIAMOND COLLECTED!");
        Debug.Log("LEVEL COMPLETE!");

        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        // Stop the player/gameplay
        Time.timeScale = 0f;

        // Hide the diamond
        gameObject.SetActive(false);
    }
}

