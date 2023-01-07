using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Plant : MonoBehaviour
{
    protected abstract PlantType plantType { get; set; }
    public virtual TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType) {
        return TileInput.EffectType.Growth;
    }

    public abstract TileInput.EffectType OnNaturalDisaster(NaturalDisasterType naturalDisasterType);
}
