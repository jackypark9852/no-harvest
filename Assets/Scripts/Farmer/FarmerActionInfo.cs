using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FarmerActionInfo
{
    Vector2Int centerTileCoordinate { get; set; }
    ShapeType shapeType { get; set; }
    PlantType plantType { get; set; }
}
