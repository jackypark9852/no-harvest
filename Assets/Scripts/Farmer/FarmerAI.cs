using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Common;
using UnityEngine;

public class FarmerAI : MonoBehaviour
{
    public Grid grid;
    public List<ShapeData> shapeDataList;

    // select a random ShapeData from shapeDataList
    ShapeData GetRandomShapeData()
    {
        int randomIndex = Random.Range(0, shapeDataList.Count);
        return shapeDataList[randomIndex];
    }
    public void SubmitFarmerActionInfo()
    {
        GameManager.Instance.SetFarmerActionInfo(GenerateFarmerActionInfo());
    }

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

    Vector2Int FindOptimalPlacementCoordinate(ShapeData shapeData)
    {
        List<Vector2Int> emptyTileCoordinates = GetEmptyTileCoordinates();

        // randomize the order of elements in emptyTileCoordinates 
        for (int i = 0; i < emptyTileCoordinates.Count; i++)
        {
            int randomIndex = Random.Range(0, emptyTileCoordinates.Count);
            Vector2Int temp = emptyTileCoordinates[i];
            emptyTileCoordinates[i] = emptyTileCoordinates[randomIndex];
            emptyTileCoordinates[randomIndex] = temp;
        }

        int maxPlaceableTileCount = 0;
        Vector2Int optimalPlacementCoordinate = new Vector2Int(0, 0); 
        // find coordinate with largest Placeable Tile Count
        foreach(Vector2Int coordinate in emptyTileCoordinates){
            int placeableTileCount = GetPlaceableTileCount(shapeData, coordinate, emptyTileCoordinates);
            // check if current coordinate has larger Placeable Tile Count than maxPlaceableTileCount
            if (placeableTileCount> maxPlaceableTileCount)
            {
                // if so, update maxPlaceableTileCount
                maxPlaceableTileCount = placeableTileCount; 
                // update optimalPlacementCoordinate
                optimalPlacementCoordinate = coordinate;
            }
        }
        return optimalPlacementCoordinate; 
    }
        
    int GetPlaceableTileCount(ShapeData shapeData, Vector2Int placementCoordinate, List<Vector2Int> emptyTileCoordinates)
    {
        int placeableTileCount = 0;
        Vector2Int[] affectedTiles = shapeData.affectedTiles;
        foreach (Vector2Int affectedTile in affectedTiles)
        {
            Vector2Int tileCoordinate = placementCoordinate + affectedTile;
            if (emptyTileCoordinates.Contains(tileCoordinate))
            {
                placeableTileCount++;
            }
        }
        return placeableTileCount;
    }

    PlantType GetRandomPlantType()
    {
        int randomIndex = Random.Range(0, System.Enum.GetNames(typeof(PlantType)).Length);
        return (PlantType)randomIndex;
    }

    public FarmerActionInfo GenerateFarmerActionInfo()
    {
        ShapeData shapeData = GetRandomShapeData();
        Vector2Int placementCoordinate = FindOptimalPlacementCoordinate(shapeData);
        PlantType plantType = GetRandomPlantType();
        return new FarmerActionInfo(placementCoordinate, shapeData, plantType);
    }
}
