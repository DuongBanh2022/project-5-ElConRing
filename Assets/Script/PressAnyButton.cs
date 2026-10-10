using UnityEngine;
using UnityEngine.InputSystem;

public class PressaAnyButton : MonoBehaviour
{
    [SerializeField] private GameObject pressAnyButton;
    [SerializeField] private GameObject mainMenu;

    private bool started = false;

    void Start()
    {
        pressAnyButton.SetActive(true);
        mainMenu.SetActive(false);
    }

    void Update()
    {
        if (started)
            return;

        // Keyboard
        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            ShowMainMenu();
            return;
        }

        // Mouse
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShowMainMenu();
            return;
        }

        // Gamepad
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame ||
                Gamepad.current.buttonNorth.wasPressedThisFrame ||
                Gamepad.current.buttonEast.wasPressedThisFrame ||
                Gamepad.current.buttonWest.wasPressedThisFrame ||
                Gamepad.current.startButton.wasPressedThisFrame)
            {
                ShowMainMenu();
            }
        }
    }

    private void ShowMainMenu()
    {
        started = true;

        pressAnyButton.SetActive(false);
        mainMenu.SetActive(true);
    }
}