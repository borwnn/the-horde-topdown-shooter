using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("Panel References")]
    [Tooltip("Drag your Pause Menu Panel GameObject here.")]
    public GameObject pauseMenuPanel;
    [Tooltip("Drag your Game Over Panel GameObject here.")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    [Header("Button References")]
    [Tooltip("The 'Load Game' button on the pause menu.")]
    public Button pauseLoadGameButton;
    [Tooltip("The 'Load Game' button on the game over menu.")]
    public Button gameOverLoadGameButton;
    public Button gameWinMenuGameButton;

    private bool isGamePaused = false;

    void Awake()
    {
        Time.timeScale = 1f;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ((gameOverPanel != null && gameOverPanel.activeInHierarchy) ||
                (gameWinPanel != null && gameWinPanel.activeInHierarchy))
            {
                return;
            }

            isGamePaused = !isGamePaused;
            TogglePauseMenu(isGamePaused);
        }
    }

    public void TogglePause()
    {
        if (gameOverPanel != null && gameOverPanel.activeInHierarchy)
        {
            return;
        }

        isGamePaused = !isGamePaused;
        TogglePauseMenu(isGamePaused);
    }

    private void TogglePauseMenu(bool shouldPause)
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(shouldPause);
            Time.timeScale = shouldPause ? 0f : 1f;
            if (shouldPause && pauseLoadGameButton != null)
            {
                pauseLoadGameButton.interactable = SaveSystem.SaveFileExists();
            }
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            if (gameOverLoadGameButton != null)
            {
                gameOverLoadGameButton.interactable = SaveSystem.SaveFileExists();
            }
        }
    }

// added code
    public void ShowGameWin()
    {
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        TogglePause();
    }

    public void SaveGame()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.SaveGame();
            if (pauseLoadGameButton != null)
            {
                pauseLoadGameButton.interactable = true;
            }
        }
    }

    public void LoadGame()
    {
        if (GameManager.instance != null)
        {
            Time.timeScale = 1f;
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            GameManager.instance.LoadGame();
        }
    }

    public void MainMenu()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBackgroundMusic();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}

