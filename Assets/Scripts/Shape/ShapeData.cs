using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeData", menuName = "ShapeData")]
public class ShapeData : ScriptableObject
{
    public ShapeType shapeType;
    public Vector2Int[] affectedTiles;
}
