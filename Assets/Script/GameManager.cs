using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int totalScore = 0;
    private int totalHits = 0;
    private int totalShots = 0;
    // public float gameDuration = 30f;
    // private float elapsedTime = 0f;

    public int winScore = 100;
    public static int FinalScore = 0;
    // public GameObject winPanel;
    // public TMP_Text finalScoreText;
    
    [Header("Game Start Settings")]
    public GameObject startTextObject; 
    
    private bool gameStarted = false;
    private bool gameEnded = false;
    public bool GameStarted { get { return gameStarted; } }
    public bool GameEnded { get { return gameEnded; } }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!gameStarted && !gameEnded && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        // if (gameStarted)
        // {
        //     elapsedTime += elapsedTime.deltaTime;
        //     if (elapsedTime >= gameDuration)
        //     {
        //         EndGame();
        //     }
        // }
    }

    void StartGame()
    {
        gameStarted = true;
        gameEnded = false;
        // elapsedTime = 0f;
        if (startTextObject != null) 
        {
            startTextObject.SetActive(false);
        }
    }

    public void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameStarted = false;
        
        float accuracy = totalShots == 0 ? 0 : ((float)totalHits / totalShots) * 100f;
        Debug.Log($"The Game is over! Final Score: {totalScore} | Accuracy: {accuracy:F1}% | Hits: {totalHits}/{totalShots}");
        if (totalScore >= winScore)
        {
            SceneManager.LoadScene("WinScene");
        } else
        {
            SceneManager.LoadScene("GameOver");      
        }
    }

    // Called when a target is hit
    public void AddScore(int score)
    {
        if (!gameStarted) return; // Don't count score if game hasn't started
        
        totalScore += score;
        FinalScore = totalScore;
        totalHits++;
        UpdateUI();
        // CheckWinCondition();
    }
    void CheckWinCondition()
    {
        // if ( !gameEnded && totalScore >= winScore)
        // {
        //     WinScene();
        // }
    }

    void WinScene()
    {
        gameEnded = true;
        gameStarted = false;
        FinalScore = totalScore;

        Debug.Log($"You win: " + totalScore);
        SceneManager.LoadScene("WinScene");
    }
    // Optional: call this for every shot fired
    public void RegisterShot()
    {
        if (!gameStarted) return; // Don't count shots if game hasn't started
        
        totalShots++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Example: log total score and accuracy
        float accuracy = totalShots == 0 ? 0 : ((float)totalHits / totalShots) * 100f;
        Debug.Log($"Total Score: {totalScore} | Accuracy: {accuracy:F1}%");
    }
}
