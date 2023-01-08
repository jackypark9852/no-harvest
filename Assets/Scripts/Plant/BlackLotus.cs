using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackLotus : Plant
{
    protected override PlantType plantType { get => PlantType.BlackLotus; set => plantType = value; }

    public override TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType)
    {
        switch (naturalDisasterType)
        {
            case NaturalDisasterType.Fire or NaturalDisasterType.Meteorite or NaturalDisasterType.Tsunami or NaturalDisasterType.Blizzard:
                return TileInput.EffectType.Growth;
            default:
                return TileInput.EffectType.Destroyed;
        }
    }
}
