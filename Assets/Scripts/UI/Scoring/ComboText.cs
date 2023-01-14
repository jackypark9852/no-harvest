using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateScoreText()
    {
        text.text = $"{string.Format("{0:0.0}", ScoreManager.Instance.comboMultiplier)}x";
    }
}
