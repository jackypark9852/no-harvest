using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : Plant
{
    protected override PlantType plantType { get => PlantType.SunFlower; set => plantType = value; }
    public override TileInput.EffectType OnNaturalDisaster(NaturalDisasterType naturalDisasterType)
    {
        switch(naturalDisasterType)
        {
            case NaturalDisasterType.Fire:
                return TileInput.EffectType.Growth;
            default:
                OnPlantDestroyed();
                return TileInput.EffectType.Destroyed; 
        }
    }
}
