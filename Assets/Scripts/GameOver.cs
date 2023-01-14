using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text scoreText2;
    
    public void StartGameOver()
    {
        gameOverMenu.SetActive(true);
        scoreText.text = $"You said no to {RoundManager.Instance.roundNum} days of harvest";
        scoreText2.text = $"Score: {ScoreManager.Instance.score}";
    }
}
