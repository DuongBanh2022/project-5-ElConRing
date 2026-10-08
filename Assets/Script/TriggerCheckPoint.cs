using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int checkpointNumber = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("CHECKPOINT: SOMETHING ENTERED");

        if (other.CompareTag("Player"))
        {
            Debug.Log("CHECKPOINT: PLAYER ENTERED!");

            PlayerPrefs.SetInt("A4Checkpoint", checkpointNumber);
            PlayerPrefs.Save();

            Debug.Log("CHECKPOINT SAVED: " + checkpointNumber);
        }
    }
}


