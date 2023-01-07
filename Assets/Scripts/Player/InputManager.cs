using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngineInternal;

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
            if (selectedTile is not null)
            {
                grid.GetTileInput(selectedTile).isSelected = false;
            }
            selectedTile = value;
            grid.GetTileInput(selectedTile).isSelected = true;
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
            if (hoveredTile is not null)
            {
                grid.GetTileInput(hoveredTile).isHovered = false;
            }
            hoveredTile = value;
            grid.GetTileInput(hoveredTile).isHovered = true;
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
        PlayerActionInfo confirmedAction = new PlayerActionInfo(selectedTile.GetCoords(),naturalDisasterType,true);
        confirmedActions.Add(confirmedAction);
        grid.ApplyConfirmedActionOnTiles(confirmedActions);
        selectedTile = null;
    }

    public void Reset()
    {
<<<<<<< HEAD
        Debug.Log("InputManager Reset");      
    }

    public void EndTurn()
    {
        GameManager.Instance.EndPlayerTurn();
=======
        Debug.Log("InputManager Reset")        
>>>>>>> e0cf3bf72516176f2f4d222c84a794f22b8122f5
    }
}