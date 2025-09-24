using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerTG : MonoBehaviour
{
    public string sceneName;
    public GameObject gameOverScreen;

    //////////////////////////////////////////////////////////

    private void Start()
    {
        gameOverScreen.SetActive(false);
    }

    //////////////////////////////////////////////////////////
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Gameplay()
    {
        SceneManager.LoadScene("G_Gameplay");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("G_MainMenu");
    }
    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        //Application.Quit();
        Debug.LogWarning("QUIT GAME");
    }
}
