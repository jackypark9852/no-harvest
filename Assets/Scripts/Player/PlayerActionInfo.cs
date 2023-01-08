using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerActionInfo
{
    public Vector2Int centerTileCoordinate { get; set; }
    public NaturalDisasterType naturalDisasterType { get; set; }
    public ActionInputType actionInputType { get; set; }

    public PlayerActionInfo(Vector2Int centerTileCoordinate, NaturalDisasterType naturalDisasterType, ActionInputType actionInputType)
    {
        this.centerTileCoordinate = centerTileCoordinate;
        this.naturalDisasterType = naturalDisasterType;
        this.actionInputType = actionInputType;
    }

    public override string ToString()
    {
        return $"PlayerActionInfo: Center Tile Coordinate: {centerTileCoordinate}, Natural Disaster Type: {naturalDisasterType.ToString()}, Action Input Type: {actionInputType.ToString()}";
    }
}

public enum ActionInputType
{
    Confirmed,
    Selected,
    Hovered,
}
