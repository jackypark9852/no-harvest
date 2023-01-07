using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NaturalDisasterData", menuName = "NaturalDisasterData")]
public class NaturalDisasterData : ScriptableObject
{
    NaturalDisasterType type { get; set; }
    ShapeType shapeType { get; set; }
    uint manaCost { get; set; }
}
