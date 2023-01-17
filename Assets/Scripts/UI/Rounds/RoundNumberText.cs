using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundNumberText : MonoBehaviour
{
    TMP_Text text;
    
    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateRoundNumberText()
    {
        text.text = $"DAY {RoundManager.Instance.roundNum}";
    }
}
