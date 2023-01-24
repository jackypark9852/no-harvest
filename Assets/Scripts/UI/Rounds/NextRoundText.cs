using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextRoundText : MonoBehaviour
{
    TMP_Text text;
    int plantCountPerPlantShape = 4;  // Hard-coded

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateNextRoundText()
    {
        text.text = $"<color=#c80000>{RoundManager.Instance.roundInfos[RoundManager.Instance.roundNum].plantsToPlant.Count * plantCountPerPlantShape}</color> plants spawn next round";
    }
}
