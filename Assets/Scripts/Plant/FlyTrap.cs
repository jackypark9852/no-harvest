using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrap : Plant
{
    protected override PlantType plantType { get => PlantType.FlyTrap; set => plantType = value; }

    public override TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType)
    {
        switch (naturalDisasterType)
        {
            case NaturalDisasterType.Blizzard:
                return TileInput.EffectType.Growth;
            default:
                return TileInput.EffectType.Destroyed;
        }
    }
}
