using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningManagerTG : MonoBehaviour
{
    public GameObject winScreen;

    void Start()
    {
        // Pastikan winScreen mati di awal
        if (winScreen != null)
            winScreen.SetActive(false);
    }

    public void ShowWin()
    {
        if (winScreen != null)
            winScreen.SetActive(true);

        // Pause game
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu(string menuScene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
