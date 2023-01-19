using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameMenuController : MonoBehaviour
{
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject leaderBoardMenu;

    public void ShowGameOver()
    {
        DisableAllPostGameMenus();
        gameOverMenu.SetActive(true); // Show the gameOver menu
    }

    public void showSubmitScoreMenu(){
        DisableAllPostGameMenus();
        leaderBoardMenu.SetActive(true); // Show the leaderboard
    }

    public void DisableAllPostGameMenus(){
        foreach( Transform child in transform )
        {
            child.gameObject.SetActive( false );
        }
    }
}
