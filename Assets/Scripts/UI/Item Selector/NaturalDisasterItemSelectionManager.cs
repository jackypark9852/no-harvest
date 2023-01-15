using System.Collections.Generic;
using UnityEngine;

public class NaturalDisasterItemSelectionManager : ItemSelectionManager<NaturalDisasterType>
{
    [SerializeField] List<NaturalDisasterType> unlockedNaturalDisasterTypes = new List<NaturalDisasterType> { NaturalDisasterType.Fire };

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitNaturalDisasterUnlocks()
    {
        if (RoundManager.Instance.roundNum != 1)  // Hard-coded
        {
            return;
        }
        foreach (NaturalDisasterType naturalDisasterType in itemToDisplayMap.Keys)
        {
            LockNaturalDisasterType(naturalDisasterType);
        }
        foreach (NaturalDisasterType naturalDisasterType in unlockedNaturalDisasterTypes)
        {
            UnlockNaturalDisasterType(naturalDisasterType);
        }

        if(defaultSelectedItemDisplay is not null) {
            SelectItem(defaultSelectedItemDisplay); // Select item and activate item frame
        }
    }

    public override void SelectItem(ItemDisplay<NaturalDisasterType> itemDisplay) 
    { 
        NaturalDisasterDisplay naturalDisasterDisplay = itemDisplay as NaturalDisasterDisplay; // Polymorphism to access locked property
        if(GameManager.Instance.state == GameState.PlayerTurn && !naturalDisasterDisplay.IsLocked()){
            base.SelectItem(itemDisplay);
            InputManager.Instance.NaturalDisasterType_ = naturalDisasterDisplay.item.item;
            InputManager.Instance.UpdateHover();
        }
    }
    
    public void UnlockRoundNaturalDisasterTypes()
    {
        if (RoundManager.Instance.roundNum >= RoundManager.Instance.roundInfos.Count)
        {
            return;
        }
        List<NaturalDisasterType> roundNaturalDisasterTypes = RoundManager.Instance.roundInfos[RoundManager.Instance.roundNum].naturalDisasterUnlocks;
        foreach (NaturalDisasterType roundNaturalDisasterType in roundNaturalDisasterTypes)
        {
            UnlockNaturalDisasterType(roundNaturalDisasterType);
        }
    }

    public void LockNaturalDisasterType(NaturalDisasterType naturalDisasterType)
    {
        UnlockOrLockNaturalDisasterType(naturalDisasterType, true);
    }
    public void UnlockNaturalDisasterType(NaturalDisasterType naturalDisasterType)
    {
        UnlockOrLockNaturalDisasterType(naturalDisasterType, false);
    }
    private void UnlockOrLockNaturalDisasterType(NaturalDisasterType naturalDisasterType, bool locked)
    {
        NaturalDisasterDisplay naturalDisasterDisplay = (NaturalDisasterDisplay)itemToDisplayMap[naturalDisasterType];
        naturalDisasterDisplay.UnlockOrLockDisplay(locked);
    }
}
