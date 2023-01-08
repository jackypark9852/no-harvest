using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeData", menuName = "ShapeData")]
public class ShapeData : ScriptableObject
{
    public ShapeType shapeType;
    public Vector2Int[] affectedTiles;

    public ShapeData(ShapeType shapeType, Vector2Int[] affectedTiles)
    {
        this.shapeType = shapeType;
        this.affectedTiles = affectedTiles;
    }
}
