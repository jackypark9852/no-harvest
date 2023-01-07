using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        return tileInputs[tile.GetCoords()];
    }

    // TODO: might overwrite tile highlights incorrectly, fix later
    public void ApplyConfirmedActionOnTiles(List<PlayerActionInfo> confirmedActions)
    {
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
            Debug.Log(tile.GetCoords());
            TileInput tileInput = GetTileInput(tile);
            if(tile.plant)
            {
                tileInput.effectType = tile.plant.GetEffectType(confirmedAction.naturalDisasterType);
            } else
            {
                tileInput.effectType = TileInput.EffectType.Destroyed;
            }
            tileInput.isBlinking = !confirmedAction.confirmed;
        }
    }
}
