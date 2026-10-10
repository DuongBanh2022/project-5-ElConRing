using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPointSaver : MonoBehaviour
{
    [SerializeField] private int checkpointNumber = 0;

    private void Start()
    {
        int loadedCheckpoint = PlayerPrefs.GetInt("A4Checkpoint", 0);
        Debug.Log("Loaded checkpoint: " + loadedCheckpoint);
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            PlayerPrefs.SetInt("A4Checkpoint", checkpointNumber);
            PlayerPrefs.Save();

            Debug.Log("Saved checkpoint: " + checkpointNumber);
        }
    }
}