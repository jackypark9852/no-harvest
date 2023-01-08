using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverMenu;

    public void StartGameOver()
    {
        gameOverMenu.SetActive(true);
    }
}
