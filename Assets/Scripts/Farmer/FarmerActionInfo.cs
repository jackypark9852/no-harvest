using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FarmerActionInfo
{
    public Vector2Int centerTileCoordinate { get; set; }
    public ShapeData shapeData { get; set; }
    public PlantType plantType { get; set; }

    public FarmerActionInfo(Vector2Int centerTileCoordinate, ShapeData shapeData, PlantType plantType)
    {
        this.centerTileCoordinate = centerTileCoordinate;
        this.shapeData = shapeData;
        this.plantType = plantType;
    }

    public override string ToString()
    {

        return ("FarmerActionInfo: centerTileCoordinate: " + centerTileCoordinate + ", shapeData: " + shapeData + ", plantType: " + plantType);
    }
}
