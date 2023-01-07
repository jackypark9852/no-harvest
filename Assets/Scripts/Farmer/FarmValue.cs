using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmValue : MonoBehaviour
{
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
        if (tile is null)
        {
            return 0;
        }
        return 1;
    }
}
