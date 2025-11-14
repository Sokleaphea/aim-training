using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startTextPanel;
    
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameManager.Instance;
        
        if (startTextPanel != null)
        {
            startTextPanel.SetActive(true);
            
            Text startText = startTextPanel.GetComponent<Text>();
            if (startText != null)
            {
                startText.text = "Press Space to Start the Game";
            }
        }
    }
    
    void Update()
    {
        if (gameManager != null && Input.GetKeyDown(KeyCode.Space) && !gameManager.IsGameStarted)
        {
            StartGame();
        }
    }
    
    void StartGame()
    {
        if (gameManager != null)
        {
            gameManager.StartGame();
            
            if (startTextPanel != null)
            {
                startTextPanel.SetActive(false);
            }
        }
    }
}
