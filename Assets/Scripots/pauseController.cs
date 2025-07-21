using UnityEngine;

public class pauseController : MonoBehaviour
{
    public GameObject pauseMenu; // Drag your Pause Menu UI here in the Inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        isPaused = true;

        Cursor.lockState = CursorLockMode.None; // Show the cursor
        Cursor.visible = true;
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked; // Hide the cursor
        Cursor.visible = false;
    }
}
