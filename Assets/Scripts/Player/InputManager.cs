using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    Tile selectedTile = null;
    public Tile SelectedTile
    {
        get
        {
            return selectedTile;
        }
        set
        {
            selectedTile = value;
            grid.ApplyConfirmedActionOnTiles(confirmedActions);
        }
    }
    Tile hoveredTile = null;
    public Tile HoveredTile
    {
        get
        {
            return hoveredTile;
        }
        set
        {
            hoveredTile = value;
            grid.ApplyConfirmedActionOnTiles(confirmedActions);
        }
    }

    List<PlayerActionInfo> confirmedActions = new List<PlayerActionInfo>();
    NaturalDisasterType naturalDisasterType { get; set; }

    Grid grid;

    void Awake()
    {
        grid = FindObjectOfType<Grid>();

        naturalDisasterType = NaturalDisasterType.Meteorite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Confirm();
        }
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
