using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NaturalDisasterData", menuName = "NaturalDisasterData")]
public class NaturalDisasterData : ScriptableObject
{
    public NaturalDisasterType type;
    public ShapeData shapeData;
    public uint manaCost;
}
