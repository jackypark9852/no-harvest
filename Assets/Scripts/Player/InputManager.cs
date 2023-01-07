using System;
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
            UpdateSelect();
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
            UpdateHover();
        }
    }

    List<PlayerActionInfo> actions = new List<PlayerActionInfo>();
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

    private void UpdateHover()
    {
        if (hoveredTile is null)
        {
            return;
        }
        PlayerActionInfo action = new PlayerActionInfo(hoveredTile.GetCoords(), naturalDisasterType, ActionInputType.Hovered);
        AddAction(action);
        grid.ApplyConfirmedActionsOnTiles(actions);
    }

    private void UpdateSelect()
    {
        if (selectedTile is null)
        {
            return;
        }
        PlayerActionInfo action = new PlayerActionInfo(selectedTile.GetCoords(), naturalDisasterType, ActionInputType.Selected);
        AddAction(action);
        grid.ApplyConfirmedActionsOnTiles(actions);
    }

    private void Confirm()
    {
        bool confirmed = ConfirmAction();
        if (!confirmed)
        {
            return;
        }
        grid.ApplyConfirmedActionsOnTiles(actions);
        selectedTile = null;
    }

    private void AddAction(PlayerActionInfo action)
    {
        ActionInputType actionInputType = action.actionInputType;
        ActionInputType? prevActionInputType = GetPrevActionInputType();
        switch (action.actionInputType)
        {
            case ActionInputType.Confirmed:
                throw new ArgumentException("This should not happen?");
            case ActionInputType.Selected:
                switch (prevActionInputType)
                {
                    case ActionInputType.Hovered or ActionInputType.Selected:
                        actions[actions.Count - 1] = action;
                        break;
                    case ActionInputType.Confirmed or null:
                        actions.Add(action);
                        break;
                }
                break;
            case ActionInputType.Hovered:
                switch (prevActionInputType)
                {
                    case ActionInputType.Hovered:
                        actions[actions.Count - 1] = action;
                        break;
                    case ActionInputType.Confirmed or null:
                        actions.Add(action);
                        break;
                }
                break;
        }
    }

    private ActionInputType? GetPrevActionInputType()
    {
        if (actions.Count == 0)
        {
            return null;
        }
        return actions[actions.Count - 1].actionInputType;
    }

    private bool ConfirmAction()
    {
        if (actions.Count == 0 || actions[actions.Count - 1].actionInputType != ActionInputType.Selected)
        {
            return false;
        }
        actions[actions.Count - 1] = new PlayerActionInfo(actions[actions.Count - 1].centerTileCoordinate, actions[actions.Count - 1].naturalDisasterType, ActionInputType.Confirmed);
        return true;
    }
}