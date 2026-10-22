
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public TMP_Text scoreText;

    public GameObject endGamePanel;
    public TMP_Text endGameText;

    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateScoreText();

        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    public void AddScore(int points)
    {
        if (gameEnded)
            return;

        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void GameOver()
    {
        EndGame("GAME OVER");
    }


    public void CheckWin()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        EndGame("YOU WIN!");
    }

    void EndGame(string message)
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (endGamePanel != null)
            endGamePanel.SetActive(true);
        else
            Debug.LogError("End Game Panel is NOT assigned!");

        if (endGameText != null)
            endGameText.text = message;
        else
            Debug.LogError("End Game Text is NOT assigned!");

        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

}
