using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Common;
using UnityEngine;

public class FarmerAI : MonoBehaviour
{
    public Grid grid;
    public List<ShapeData> shapeDataList;

    List<Vector2Int> GetEmptyTileCoordinates()
    {
        Dictionary<Vector2Int, Tile> tiles = grid.Tiles; 
        List<Vector2Int> emptyTileCoordinates = new List<Vector2Int>();
        // Write a loop that iterates through the dictionary and check plant property
        foreach (KeyValuePair<Vector2Int, Tile> entry in tiles)
        {
            // Get the Tile object from the value of the KeyValuePair
            Tile tile = entry.Value;

            // Check if the plant property is null
            if (tile.plant == null)
            {
                // If it is null, add the key (coordinate) to the list of empty tile coordinates
                emptyTileCoordinates.Add(entry.Key);
            }
        }
        return emptyTileCoordinates; 
    }
}
