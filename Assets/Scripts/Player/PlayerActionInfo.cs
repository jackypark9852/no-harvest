using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerActionInfo
{
    public Vector2Int centerTileCoordinate { get; set; }
    public NaturalDisasterType naturalDisasterType { get; set; }
    public bool confirmed { get; set; }
}
