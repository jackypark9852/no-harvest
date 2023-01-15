using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSpriteAnimation : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro text; 
    public void SetScore(int score) {
        text.text = score.ToString(); 
    }
}
