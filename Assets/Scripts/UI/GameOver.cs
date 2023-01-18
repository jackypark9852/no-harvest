using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text scoreText2;
    
    private void OnEnable() {
        Debug.Log("Game Over Screen enabled");
        scoreText.text = $"You said no to {RoundManager.Instance.roundNum} days of harvest";
        scoreText2.text = $"Score: {ScoreManager.Instance.score}";
    }
}
