using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FarmerActionInfo
{
    Vector2Int centerTileCoordinate { get; set; }
    ShapeData shapeData { get; set; }
    PlantType plantType { get; set; }

    public FarmerActionInfo(Vector2Int centerTileCoordinate, ShapeData shapeData, PlantType plantType)
    {
        this.centerTileCoordinate = centerTileCoordinate;
        this.shapeData = shapeData;
        this.plantType = plantType;
    }

    public string ToString()
    {

        return ("FarmerActionInfo: centerTileCoordinate: " + centerTileCoordinate + ", shapeData: " + shapeData + ", plantType: " + plantType);
    }
}
