using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsMenuPanel;

    void Start()
    {
        // Hides the cursor at start of game
        Cursor.visible = false;

        // Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleSettingsMenu();
        }

        // Ensure cursor state matches menu
        if (settingsMenuPanel != null)
        {
            if (settingsMenuPanel.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f; // Pause the game when settings menu is open
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f; // Resume the game when settings menu is closed
            }
        }
    }


    public void ToggleSettingsMenu()
    {
        if (settingsMenuPanel != null)
        {
            settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);

            if (settingsMenuPanel.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f; // Pause the game when settings menu is open
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f; // Resume the game when settings menu is closed
            }
        }
    }
}