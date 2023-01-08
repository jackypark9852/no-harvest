using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WaterMellon : Plant
{
    protected override PlantType plantType { get => PlantType.WaterMellon; set => plantType = value;}

    public override TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType)
    {
        switch (naturalDisasterType)
        {
            case NaturalDisasterType.Tsunami:
                return TileInput.EffectType.Growth;
            default:
                return TileInput.EffectType.Destroyed;
        }
    }
}
