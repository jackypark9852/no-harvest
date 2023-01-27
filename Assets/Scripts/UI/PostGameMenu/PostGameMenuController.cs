using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameMenuController : MonoBehaviour
{
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject leaderBoardMenu;

    [SerializeField] int gameOverDelayMs = 500;
    Grid grid;

    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    public async void ShowGameOver()
    {
        DisableAllPostGameMenus();
        await grid.PlantOnEveryTile();
        await UniTask.Delay(gameOverDelayMs);
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
