using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    [Header("Time Components")]
    [SerializeField] private float timeRemaining = 10;
    public float maxTime = 60f;

    [SerializeField] private bool timerRunning = false;

    [Header("Timers in Scene")]
    public TextMeshPro mazeTimerText;

    //Interacts with
    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.getGameStart() && !timerRunning)
        {
            StartTimer();
        }

        if (timerRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText(mazeTimerText);

            }
            else
            {
                timerRunning = false;
                gameManager.setGameState(false);
                GameOverScreen();
            }
        }
    }

    void StartTimer()
    {
        timerRunning = true;
        timeRemaining = maxTime;
    }

    void GameOverScreen()
    {
        ScoreManager.Instance.Score = 0;
        gameManager.setGameState(false);
        ScoreManager.Instance.State = gameManager.victoryOrNot;

        SceneManager.LoadScene("EndScene");
    }

    private void UpdateTimerText(TextMeshPro selectedText)
    {
        selectedText.text = "Time left: " + Math.Ceiling(timeRemaining).ToString();
    }
}
