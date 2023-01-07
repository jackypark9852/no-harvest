using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerActionInfo
{
    Vector2Int centerTileCoordinate { get; set; }
    NaturalDisasterType naturalDisasterType { get; set; }
    bool confirmed { get; set; }
}
