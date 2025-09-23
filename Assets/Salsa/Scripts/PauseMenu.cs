using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public string sceneName;

    public GameObject pauseMenuUI;
    public Button pauseButton;

    void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
    }

    private void start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty! Assign a scene name in the Inspector.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
