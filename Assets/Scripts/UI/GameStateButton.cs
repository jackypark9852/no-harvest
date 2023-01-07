using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    
public class GameStateButton : MonoBehaviour
{
    public TMP_Text buttonText;

    public void UpdateText()
    {
        GameState state = GameManager.Instance.State;
        string stateString = $"GameState: {state.ToString()}";
        buttonText.text = stateString;
    }
}
