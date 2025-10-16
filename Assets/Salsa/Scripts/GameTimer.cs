using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float startTime = 10f; // waktu awal (10 detik)
    private float timeRemaining;
    private bool isGameOver = false;

    public GameOverManager gameOverManager;
    public TextMeshProUGUI timerText;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!isGameOver) // timer hanya jalan kalau belum game over
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                TriggerGameOver();
            }
        }
    }

    void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time: " + seconds.ToString();
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        timeRemaining = 0;
        UpdateTimerUI();
        gameOverManager.ShowGameOver();
        Debug.Log("⏰ Waktu habis! Game Over!");
    }

    public void ResetTimer()
    {
        timeRemaining = startTime;
        isGameOver = false;
        UpdateTimerUI();
    }
}
