using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    [HideInInspector] public int roundNum { get; private set; } = 0;

    [HideInInspector] public List<RoundInfo> roundInfos2 = new List<RoundInfo>
    {
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {}, new List<NaturalDisasterType> {NaturalDisasterType.Fire}),
        new RoundInfo(new List<int> {}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1}, new List<NaturalDisasterType> {NaturalDisasterType.Tsunami}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2}, new List<NaturalDisasterType> {NaturalDisasterType.Meteorite}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {NaturalDisasterType.Blizzard}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {NaturalDisasterType.Lightning}),
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
    };

    public void IncrementRound()
    {
        roundNum++;
    }
}

[Serializable]
public struct RoundInfo
{
    public List<PlantType> plantsToPlant;
    public List<NaturalDisasterType> naturalDisasterUnlocks;

    public RoundInfo(List<PlantType> plantsToPlant, List<NaturalDisasterType> naturalDisasterUnlocks)
    {
        this.plantsToPlant = plantsToPlant;
        this.naturalDisasterUnlocks = naturalDisasterUnlocks;
    }

    public RoundInfo(List<int> plantIntsToPlant, List<NaturalDisasterType> naturalDisasterUnlocks)
    {
        plantsToPlant = new List<PlantType>();
        foreach (int plantInt in plantIntsToPlant)
        {
            plantsToPlant.Add((PlantType)plantInt);
        }
        this.naturalDisasterUnlocks = naturalDisasterUnlocks;
    }
}
