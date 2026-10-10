using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    private void Update()
    {
        // Press F10 to reset saved data during testing.
        if (Keyboard.current != null &&
            Keyboard.current.f10Key.wasPressedThisFrame)
        {
            ResetSave();
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("HasLastLocation");
        PlayerPrefs.DeleteKey("LastX");
        PlayerPrefs.DeleteKey("LastY");
        PlayerPrefs.DeleteKey("LastZ");

        PlayerPrefs.Save();

        Debug.Log("SAVE DATA RESET.");
    }
}
