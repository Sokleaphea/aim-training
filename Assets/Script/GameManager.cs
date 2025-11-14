using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public bool IsGameStarted { get; private set; } = false;
    
    private int totalScore = 0;
    private int totalHits = 0;
    private int totalShots = 0;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void StartGame()
    {
        if (!IsGameStarted)
        {
            IsGameStarted = true;
            Debug.Log("Game Started!");
        }
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