using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int totalScore = 0;
    private int totalHits = 0;
    private int totalShots = 0;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called when a target is hit
    public void AddScore(int score)
    {
        totalScore += score;
        totalHits++;
        UpdateUI();
    }

    // Optional: call this for every shot fired
    public void RegisterShot()
    {
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
