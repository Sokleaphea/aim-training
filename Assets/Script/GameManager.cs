using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int totalScore = 0;
    private int totalHits = 0;
    private int totalShots = 0;
    
    [Header("Game Start Settings")]
    public GameObject startTextObject; // Assign the UI Text/TextMeshPro object in the inspector
    
    private bool gameStarted = false;
    public bool GameStarted { get { return gameStarted; } }

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
        // Check for space key to start the game
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        
        // Hide the start text
        if (startTextObject != null)
        {
            startTextObject.SetActive(false);
        }
    }

    // Called when a target is hit
    public void AddScore(int score)
    {
        if (!gameStarted) return; // Don't count score if game hasn't started
        
        totalScore += score;
        totalHits++;
        UpdateUI();
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
