using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NaturalDisasterType
{
    Fire,
    Meteorite, 
    Tsunami,
    Blizzard, 
    Lightning, 
}

public class NaturalDisasterUtil : Singleton<NaturalDisasterUtil>
{
    [SerializeField] List<NaturalDisasterData> naturalDisasters;
    public Dictionary<NaturalDisasterType, NaturalDisasterData> NaturalDisasterTypeToData { get; private set; } = new Dictionary<NaturalDisasterType, NaturalDisasterData>();

    void Awake()
    {
        foreach (NaturalDisasterData naturalDisaster in naturalDisasters)
        {
            NaturalDisasterTypeToData[naturalDisaster.type] = naturalDisaster;
        }
    }
}
