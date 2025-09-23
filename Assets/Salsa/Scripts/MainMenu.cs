using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;

    private void Start()
    {

    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Gameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
