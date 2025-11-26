using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Game Settings")]
    [SerializeField] private float gameTime = 60f;
    [SerializeField] private int winScore = 1000;

    private int score = 0;
    private float timeRemaining;
    private bool gameActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 0f;
        timeRemaining = gameTime;

        if (startScreen != null) startScreen.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        UpdateUI();
        LoadHighScore();
    }

    void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;
        UpdateUI();

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        gameActive = true;

        if (startScreen != null) startScreen.SetActive(false);

        if (Board.Instance != null)
            Board.Instance.enabled = true;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();

        if (score >= winScore)
        {
            WinGame();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    void GameOver()
    {
        gameActive = false;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Score: " + score;
        }

        SaveHighScore();
    }

    void WinGame()
    {
        gameActive = false;
        Time.timeScale = 0f;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Score: " + score;
        }

        SaveHighScore();
    }

    void LoadHighScore()
    {
        if (highScoreText != null)
        {
            int best = PlayerPrefs.GetInt("Match3HighScore", 0);
            highScoreText.text = "Best: " + best;
        }
    }

    void SaveHighScore()
    {
        int currentBest = PlayerPrefs.GetInt("Match3HighScore", 0);
        if (score > currentBest)
        {
            PlayerPrefs.SetInt("Match3HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
