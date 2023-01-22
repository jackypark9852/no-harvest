using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : Singleton<RoundManager>
{
    [HideInInspector] public int roundNum { get; private set; } = 0;

    internal List<RoundInfo> roundInfos = new List<RoundInfo>
    {
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        
        // Rounds 1-12
        new RoundInfo(new List<int> {}, new List<NaturalDisasterType> {NaturalDisasterType.Fire}),
        new RoundInfo(new List<int> {1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {NaturalDisasterType.Tsunami}),
        new RoundInfo(new List<int> {2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {NaturalDisasterType.Meteorite}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {NaturalDisasterType.Blizzard}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {NaturalDisasterType.Lightning}),
        new RoundInfo(new List<int> {1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2}, new List<NaturalDisasterType> {}),
        
        // Rounds 13-24
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),
        
        // Rounds 25-36
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 3}, new List<NaturalDisasterType> {}),

        // Rounds 37-48
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2, 3}, new List<NaturalDisasterType> {}),

        // Rounds 49-60
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),

        // Rounds 61-72
        new RoundInfo(new List<int> {0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),

        // Rounds 73-84
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 3}, new List<NaturalDisasterType> {}),
        
        // Rounds 85-96
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 2, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 1, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 2, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 1, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 2, 3}, new List<NaturalDisasterType> {}),

        // Rounds 97-108
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 2, 3}, new List<NaturalDisasterType> {}),

        // Rounds 109-120
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 1, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 2, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3, 3, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 1, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 2, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3, 3, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 0, 0}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {1, 1, 1}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {2, 2, 2}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3, 3, 3}, new List<NaturalDisasterType> {}),

        // Rounds 121+:
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3, 3, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {0, 1, 2, 3}, new List<NaturalDisasterType> {}),
        new RoundInfo(new List<int> {3, 3, 3}, new List<NaturalDisasterType> {}),
    };

    [SerializeField] UnityEvent OnRoundChange;

    public void IncrementRound()
    {
        roundNum++;
        OnRoundChange.Invoke();
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
