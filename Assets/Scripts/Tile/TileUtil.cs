using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public static class TileUtil
{
    static Grid grid;

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        // TODO: Find a better way to do this
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }
    public static List<Tile> GetEmptyTiles()
    {
        Dictionary<Vector2Int, Tile> tiles = grid.Tiles;
        List<Tile> emptyTiles = new List<Tile>();
        foreach (KeyValuePair<Vector2Int, Tile> entry in tiles)
        {
            // Get the Tile object from the value of the KeyValuePair
            Tile tile = entry.Value;

            // Check if the plant property is null
            if (tile.Plant == null)
            {
                // If it is null, add the key (coordinate) to the list of empty tile coordinates
                emptyTiles.Add(tile);
            }
        }

        return emptyTiles;
    }
    public static List<Vector2Int> GetEmptyTileCoordinates()
    {
        Dictionary<Vector2Int, Tile> tiles = grid.Tiles;
        List<Vector2Int> emptyTileCoordinates = new List<Vector2Int>();
        foreach (KeyValuePair<Vector2Int, Tile> entry in tiles)
        {
            // Get the Tile object from the value of the KeyValuePair
            Tile tile = entry.Value;

            // Check if the plant property is null
            if (tile.Plant == null)
            {
                // If it is null, add the key (coordinate) to the list of empty tile coordinates
                emptyTileCoordinates.Add(entry.Key);
            }
        }

        return emptyTileCoordinates;
    }
    public static List<Tile> GetAffectedTiles(Vector2Int centerCoordinate, ShapeData shapeData)
    {
        Dictionary<Vector2Int, Tile> tiles = grid.Tiles;
        List<Tile> validAffectedTiles = new List<Tile>();
        Vector2Int[] affectedTiles = shapeData.affectedTiles;
        foreach (Vector2Int affectedTile in affectedTiles)
        {
            Vector2Int tileCoordinate = centerCoordinate + affectedTile;
            if (tiles.ContainsKey(tileCoordinate))
            {
                validAffectedTiles.Add(tiles[tileCoordinate]);
            }
        }
        return validAffectedTiles;
    }
}
