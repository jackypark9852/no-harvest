using System;
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
            if (GameManager.Instance.state != GameState.PlayerTurn)
            {
                return;
            }

            if (selectedTile is not null)
            {
                grid.GetTileInput(selectedTile).isSelected = false;
            }
            selectedTile = value;
            if (selectedTile is null)
            {
                return;
            }
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
            if (GameManager.Instance.state != GameState.PlayerTurn)
            {
                return;
            }
            
            if (hoveredTile is not null)
            {
                grid.GetTileInput(hoveredTile).isHovered = false;
            }
            hoveredTile = value;
            if (hoveredTile is null)
            {
                return;
            }
            grid.GetTileInput(hoveredTile).isHovered = true;
            UpdateHover();
        }
    }

    List<PlayerActionInfo> actions = new List<PlayerActionInfo>();
    NaturalDisasterType naturalDisasterType;
    public NaturalDisasterType NaturalDisasterType_
    {
        get
        {
            return naturalDisasterType;
        }
        set
        {
            naturalDisasterType = value;
            UpdateSelect();
        }
    }

    Grid grid;

    public int mana { get; private set; }
    [SerializeField] int manaIncreasePerTurn = 1;  // May not be constant per turn?

    void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Confirm();
        }
        */
    }

    private void UpdateHover()
    {
        if (HoveredTile is null)
        {
            return;
        }
        PlayerActionInfo action = new PlayerActionInfo(HoveredTile.GetCoords(), NaturalDisasterType_, ActionInputType.Hovered);
        AddAction(action);
        grid.ApplyConfirmedActionsOnTiles(actions);
    }

    private void UpdateSelect()
    {
        if (SelectedTile is null)
        {
            return;
        }
        PlayerActionInfo action = new PlayerActionInfo(SelectedTile.GetCoords(), NaturalDisasterType_, ActionInputType.Selected);
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
        NaturalDisasterType confirmedNaturalDisasterType = actions[actions.Count - 1].naturalDisasterType;
        if (GetManaCost(confirmedNaturalDisasterType) < mana)
        {
            Debug.Log("Not enough mana, display message");
            return;
        }
        grid.ApplyConfirmedActionsOnTiles(actions);
        SelectedTile = null;
    }
    
    private uint GetManaCost(NaturalDisasterType naturalDisasterType)
    {
        return NaturalDisasterUtil.Instance.NaturalDisasterTypeToData[naturalDisasterType].manaCost;
    }

    private void IncreaseMana(int incAmount)
    {
        mana += incAmount;
        // Update mana bar
    }

    public void AddManaForTurn()
    {
        IncreaseMana(manaIncreasePerTurn);
    }

    public void Reset()
    {
        actions.Clear();
        grid.ApplyConfirmedActionsOnTiles(actions);
        SelectedTile = null;
        HoveredTile = null;
    }

    public void EndTurn()
    {
        if (GameManager.Instance.state != GameState.PlayerTurn)
        {
            return;
        }
        Confirm();
        GameManager.Instance.SetPlayerActionInfos(actions);
        GameManager.Instance.EndPlayerTurn();
    }
    private void AddAction(PlayerActionInfo action)
    {
        ActionInputType actionInputType = action.actionInputType;
        ActionInputType? prevActionInputType = GetPrevActionInputType();
        ActionInputType? prevPrevActionInputType = GetPrevPrevActionInputType();
        switch (action.actionInputType)
        {
            case ActionInputType.Confirmed:
                throw new ArgumentException("This should not happen?");
            case ActionInputType.Selected:
                switch (prevActionInputType)
                {
                    case ActionInputType.Hovered or ActionInputType.Selected:
                        switch (prevPrevActionInputType)  // Added to have hover after select
                        {
                            case ActionInputType.Selected:
                                actions.RemoveAt(actions.Count - 2);
                                break;
                        }
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
                    case ActionInputType.Selected:  // Added to have hover after select
                        actions.Add(action);
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
        if (actions.Count <= 0)
        {
            return null;
        }
        return actions[actions.Count - 1].actionInputType;
    }
    private ActionInputType? GetPrevPrevActionInputType()
    {
        if (actions.Count <= 1)
        {
            return null;
        }
        return actions[actions.Count - 2].actionInputType;
    }

    private bool ConfirmAction()
    {
        if (actions.Count >= 2)  // Added to have hover after select
        {
            if (actions[actions.Count - 2].actionInputType == ActionInputType.Selected && actions[actions.Count - 1].actionInputType == ActionInputType.Hovered)
            {
                actions.RemoveAt(actions.Count - 1);
            }
        }
        if (actions.Count == 0 || actions[actions.Count - 1].actionInputType != ActionInputType.Selected)
        {
            return false;
        }
        actions[actions.Count - 1] = new PlayerActionInfo(actions[actions.Count - 1].centerTileCoordinate, actions[actions.Count - 1].naturalDisasterType, ActionInputType.Confirmed);
        return true;
    }
}