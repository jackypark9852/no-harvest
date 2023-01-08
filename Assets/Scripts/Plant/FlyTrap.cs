using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrap : Plant
{
    protected override PlantType plantType { get => PlantType.FlyTrap; set => plantType = value; }

    public override TileInput.EffectType OnNaturalDisaster(NaturalDisasterType naturalDisasterType)
    {
        switch(naturalDisasterType)
        {
            case NaturalDisasterType.LocustSwarm:
                return TileInput.EffectType.Growth;
            default:
                OnPlantDestroyed();
                return TileInput.EffectType.Destroyed; 
        }
    }
}
