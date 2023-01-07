using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FarmerActionInfo
{
    Vector2Int centerTileCoordinate { get; set; }
    ShapeData shapeData { get; set; }
    PlantType plantType { get; set; }
}
