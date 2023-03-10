using System.Collections.Generic;
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
        GameManager.Instance.SetFarmerActionInfo(GenerateFarmerActionInfos());
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
            if (tile.Plant == null && tile.plantable)
            {
                // If it is null, add the key (coordinate) to the list of empty tile coordinates
                emptyTileCoordinates.Add(entry.Key);
            }
        }
        return emptyTileCoordinates; 
    }
    Vector2Int FindOptimalPlacementCoordinate(ShapeData shapeData)
    {
        HashSet<Vector2Int> previouslyChosenTileCoords = new HashSet<Vector2Int>();
        return FindOptimalPlacementCoordinate(shapeData, ref previouslyChosenTileCoords);
    }

    Vector2Int FindOptimalPlacementCoordinate(ShapeData shapeData, ref HashSet<Vector2Int> previouslyChosenTileCoords)
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
            int placeableTileCount = GetPlaceableTileCount(shapeData, coordinate, emptyTileCoordinates, previouslyChosenTileCoords);
            // check if current coordinate has larger Placeable Tile Count than maxPlaceableTileCount
            if (placeableTileCount> maxPlaceableTileCount)
            {
                // if so, update maxPlaceableTileCount
                maxPlaceableTileCount = placeableTileCount; 
                // update optimalPlacementCoordinate
                optimalPlacementCoordinate = coordinate;
            }
        }

        Vector2Int[] affectedTiles = shapeData.affectedTiles;
        foreach (Vector2Int affectedTile in affectedTiles)
        {
            Vector2Int tileCoordinate = optimalPlacementCoordinate + affectedTile;
            previouslyChosenTileCoords.Add(tileCoordinate);
        }
        return optimalPlacementCoordinate; 
    }
    int GetPlaceableTileCount(ShapeData shapeData, Vector2Int placementCoordinate, List<Vector2Int> emptyTileCoordinates, HashSet<Vector2Int> previouslyChosenTileCoords)
    {
        int placeableTileCount = 0;
        Vector2Int[] affectedTiles = shapeData.affectedTiles;
        foreach (Vector2Int affectedTile in affectedTiles)
        {
            Vector2Int tileCoordinate = placementCoordinate + affectedTile;
            if (emptyTileCoordinates.Contains(tileCoordinate) && !previouslyChosenTileCoords.Contains(tileCoordinate))
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
    public List<FarmerActionInfo> GenerateFarmerActionInfos()
    {
        if (RoundManager.Instance.roundNum >= RoundManager.Instance.roundInfos.Count)
        {
            // Make sure this never happens
            return Generate1RandomFarmerActionInfos();
        }
        List<PlantType> plantsToPlant = RoundManager.Instance.roundInfos[RoundManager.Instance.roundNum].plantsToPlant;
        List<FarmerActionInfo> farmerActionInfos = new List<FarmerActionInfo>();
        
        if (RoundManager.Instance.roundNum == 0 && plantsToPlant.Count == 2)  // Hard-coded
        {
            ShapeData shapeData = GetRandomShapeData();
            FarmerActionInfo farmerActionInfo1 = new FarmerActionInfo(new Vector2Int(2, 6), shapeData, plantsToPlant[0]);
            FarmerActionInfo farmerActionInfo2 = new FarmerActionInfo(new Vector2Int(6, 2), shapeData, plantsToPlant[1]);
            return new List<FarmerActionInfo> { farmerActionInfo1, farmerActionInfo2 };
        }
        HashSet<Vector2Int> previouslyChosenTileCoords = new HashSet<Vector2Int>();
        foreach (PlantType plant in plantsToPlant)
        {
            ShapeData shapeData = GetRandomShapeData();
            Vector2Int centerTileCoordinate = FindOptimalPlacementCoordinate(shapeData, ref previouslyChosenTileCoords);
            FarmerActionInfo farmerActionInfo = new FarmerActionInfo(centerTileCoordinate, shapeData, plant);
            farmerActionInfos.Add(farmerActionInfo);
        }
        return farmerActionInfos;
    }
    public List<FarmerActionInfo> Generate1RandomFarmerActionInfos()
    {
        List<FarmerActionInfo> actionInfos = new List<FarmerActionInfo>();
        ShapeData shapeData = GetRandomShapeData();
        Vector2Int placementCoordinate = FindOptimalPlacementCoordinate(shapeData);
        PlantType plantType = GetRandomPlantType();
        actionInfos.Add(new FarmerActionInfo(placementCoordinate, shapeData, plantType));
        return actionInfos;
    }
}
