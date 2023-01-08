using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TMP_Text scoreText;
    
    public void StartGameOver()
    {
        gameOverMenu.SetActive(true);
        scoreText.text = $"You said no to {GameManager.Instance.roundNum} days of harvest";
    }
}
