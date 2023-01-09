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

    // TODO: instead update this from GameManager UnityEvent
    void Update()
    {
        text.text = $"DAY {RoundManager.Instance.roundNum}";
    }
}
