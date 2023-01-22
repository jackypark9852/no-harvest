using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRow : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text rankText;
    [SerializeField] TMPro.TMP_Text nameText;
    [SerializeField] TMPro.TMP_Text scoreText;

    public void SetData(int rank, string name, int score)
    {
        rankText.text = string.Format("{0}." , rank.ToString()); 
        nameText.text = name;
        scoreText.text = string.Format("{0:00000}" , score.ToString());
    }
}
