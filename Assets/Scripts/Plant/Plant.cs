using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Plant : MonoBehaviour
{
    PlantType plantType;
    public virtual TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType) {
        return TileInput.EffectType.Growth;
    }

    public abstract TileInput.EffectType OnNaturalDisaster(NaturalDisasterType naturalDisasterType);
}
