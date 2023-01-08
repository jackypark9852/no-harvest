using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Tile> Tiles
    {
        get { return tiles; }
    }
    Dictionary<Vector2Int, TileInput> tileInputs = new Dictionary<Vector2Int, TileInput>();

    void Awake()
    {
        Tile[] tiles1D = GetComponentsInChildren<Tile>();

        foreach (Tile tile in tiles1D)
        {
            tiles[tile.GetCoords()] = tile;
            tileInputs[tile.GetCoords()] = tile.GetComponent<TileInput>();
        }
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[new Vector2Int(x, y)];
    }

    public List<Tile> GetTiles()
    {
        return tiles.Values.ToList();
    }

    public TileInput GetTileInput(Tile tile)
    {
        if (tile is null)
        {
            return null;
        }
        return tileInputs[tile.GetCoords()];
    }

    // TODO: might overwrite tile highlights incorrectly, fix later
    public void ApplyConfirmedActionsOnTiles(List<PlayerActionInfo> confirmedActions)
    {
        foreach (Tile tile in GetTiles())
        {
            GetTileInput(tile).ResetEffectType();
        }
        foreach (PlayerActionInfo confirmedAction in confirmedActions)
        {
            ApplyConfirmedActionOnTiles(confirmedAction);
        }
    }

    public void ApplyConfirmedActionOnTiles(PlayerActionInfo confirmedAction)
    {
        NaturalDisasterData naturalDisasterData = NaturalDisasterUtil.Instance.NaturalDisasterTypeToData[confirmedAction.naturalDisasterType];
        Vector2Int centerTileCoordinate = confirmedAction.centerTileCoordinate;
        ShapeData shapeData = naturalDisasterData.shapeData;

        List<Tile> affectedTiles = TileUtil.GetAffectedTiles(centerTileCoordinate, shapeData);
        foreach (Tile tile in affectedTiles)
        {
            TileInput tileInput = GetTileInput(tile);
            if (tile.Plant)
            {
                tileInput.effectType = tile.Plant.GetEffectType(confirmedAction.naturalDisasterType);
            }
            else
            {
                tileInput.effectType = TileInput.EffectType.Destroyed;
            }
            tileInput.isBlinking = confirmedAction.actionInputType == ActionInputType.Hovered;
        }
    }

    public void ApplyFarmerActionOnTiles()
    {
        List<FarmerActionInfo> farmerActionInfos = GameManager.Instance.farmerActionInfos;  
        foreach(FarmerActionInfo actionInfo in farmerActionInfos)
        {
            Vector2Int centerTile = actionInfo.centerTileCoordinate;
            ShapeData shapeData = actionInfo.shapeData;
            PlantType plantType = actionInfo.plantType;

            List<Tile> affectedTiles = TileUtil.GetAffectedTiles(centerTile, shapeData);
            foreach (Tile tile in affectedTiles)
            {
                if (tile.plantable && tile.Plant == null)
                {
                    tile.PlantNewPlant(plantType);
                }
            }
        }
        GameManager.Instance.EndFarming();
    }

    public void ApplyPlayerActionOnTiles()
    {
        List<PlayerActionInfo> playerActionInfos = GameManager.Instance.playerActionInfos;
        Debug.Log($"playerActionInfos count: {playerActionInfos.Count}"); 

        foreach(PlayerActionInfo actionInfo in playerActionInfos)
        {
            Vector2Int centerTileCoordinate = actionInfo.centerTileCoordinate;
            NaturalDisasterType naturalDisasterType = actionInfo.naturalDisasterType;
            ActionInputType actionInputType = actionInfo.actionInputType;

            Debug.Log(actionInfo.ToString());

            // Don't execute actions that are "hover" or "selected" 
            if(actionInputType == ActionInputType.Confirmed || actionInputType == ActionInputType.Selected)
            {
                if (NaturalDisasterUtil.Instance.NaturalDisasterTypeToData.ContainsKey(naturalDisasterType)) {
                    ShapeData shapeData = NaturalDisasterUtil.Instance.NaturalDisasterTypeToData[naturalDisasterType].shapeData;
                    List<Tile> affectedTiles = TileUtil.GetAffectedTiles(centerTileCoordinate, shapeData);
                    foreach (Tile tile in affectedTiles)
                    {
                        if (tile.Plant is not null)
                        {
                            Debug.Log("Grid called plant.OnNaturalDisaster");
                            tile.Plant.OnNaturalDisaster(naturalDisasterType);
                        }
                    }
                } else
                {
                    throw new Exception($"NaturalDisasterType not found: {naturalDisasterType.ToString()}"); 
                }
            }
        }
        
    }
}
