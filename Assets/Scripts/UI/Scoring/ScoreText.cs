using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    TMP_Text text;
    
    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateScoreText()
    {
        text.text = string.Format("{0:D4}", ScoreManager.Instance.score);
    }
}
