using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    Tile selectedTile = null;
    Tile hoveredTile = null;

    List<PlayerActionInfo> confirmedActions = new List<PlayerActionInfo>();
    NaturalDisasterType naturalDisasterType { get; set; }

    Grid grid;

    void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    void Update()
    {
        grid.ApplyConfirmedActionOnTiles(confirmedActions);
    }

    private void Select(Tile tile)
    {
        selectedTile = tile;
    }
    
    private void Confirm()
    {
        PlayerActionInfo confirmedAction = new PlayerActionInfo
        {
            centerTileCoordinate = selectedTile.GetCoords(),
            naturalDisasterType = naturalDisasterType,
            confirmed = true
        };
        confirmedActions.Add(confirmedAction);
        selectedTile = null;
    }
}
