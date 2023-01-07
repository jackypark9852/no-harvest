using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NaturalDisasterData", menuName = "NaturalDisasterData")]
public class NaturalDisasterData : ScriptableObject
{
    public NaturalDisasterType type { get; set; }
    public ShapeData shapeData { get; set; }
    public uint manaCost { get; set; }
}
