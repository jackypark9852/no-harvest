using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NaturalDisasterItemSelectionManager : ItemSelectionManager<NaturalDisasterType>
{
    [SerializeField] List<NaturalDisasterType> unlockedNaturalDisasterTypes = new List<NaturalDisasterType> { NaturalDisasterType.Fire };

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        foreach (NaturalDisasterType naturalDisasterType in itemToDisplayMap.Keys)
        {
            LockNaturalDisasterType(naturalDisasterType);
        }
        foreach (NaturalDisasterType naturalDisasterType in unlockedNaturalDisasterTypes)
        {
            UnlockNaturalDisasterType(naturalDisasterType);
        }
    }

    public override void SelectItem(ItemDisplay<NaturalDisasterType> itemDisplay)
    {
        base.SelectItem(itemDisplay);
        if (InputManager.Instance.NaturalDisasterType_ != itemDisplay.item.item)
        {
            // InputManager.Instance.SelectedTile = null;
        }
        InputManager.Instance.NaturalDisasterType_ = itemDisplay.item.item;
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
