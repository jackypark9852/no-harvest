using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmValue : MonoBehaviour
{
    [SerializeField] int losingFarmValue = 50;
    [SerializeField] ProgressBar progressBar;
    [SerializeField] Grid grid;

    [SerializeField] int dangerValue;
    public static event Action OnDangerValueReached;

    public int GetFarmValue(Grid grid)
    {
        List<Tile> tiles = grid.GetTiles();
        int value = 0;
        foreach (Tile tile in tiles)
        {
            value += GetTileValue(tile);
        }
        return value;
    }

    public int GetTileValue(Tile tile)
    {
        if (tile.Plant is null)
        {
            return 0;
        }
        return 1;
    }

    public void CheckGameEnd()
    {
        int farmValue = GetFarmValue(grid);
        if (farmValue >= losingFarmValue)
        {
            GameManager.Instance.EndGame(); 
        }
        else
        {
            if (farmValue >= dangerValue)
            {
                OnDangerValueReached?.Invoke();
            }
            GameManager.Instance.EndRoundTransition();
        }
    }
    
    public void UpdateFarmValueProgressBar()
    {
        // ceiling between float and 1 
        progressBar.BarValue = Mathf.Min(1, (float)GetFarmValue(grid) / losingFarmValue); 
    }

}
