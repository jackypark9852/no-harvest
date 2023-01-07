using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Plant : MonoBehaviour
{
    public TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType) {
        return TileInput.EffectType.Growth;
    }
}
