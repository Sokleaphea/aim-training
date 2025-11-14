using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WInSceneUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text scoreText;
    void Start()
    {
        scoreText.text = "Score: " + GameManager.FinalScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
