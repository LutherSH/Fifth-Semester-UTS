using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuTG : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public string sceneName;
    public bool isGamePaused;
    //public GameObject thePlayer;
    public PlayerController playerController;

    public GameObject pauseMenuUI;
    public Button pauseButton;
    public GameObject player;

    void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
        
        isGamePaused = GameIsPaused;
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
            Cursor.visible = true;
            //thePlayer.SetActive(true);
            

        }
        else
        {
            Pause();
            Cursor.visible = false;
            //thePlayer.SetActive(false);
            

        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.None;
        player.SetActive(true);
        playerController.enabled = true;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        player.SetActive(false);
        playerController.enabled = false;

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

