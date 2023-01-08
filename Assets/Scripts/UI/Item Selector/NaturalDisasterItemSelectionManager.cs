using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalDisasterItemSelectionManager : ItemSelectionManager<NaturalDisasterType>
{
    public override void SelectItem(ItemDisplay<NaturalDisasterType> itemDisplay)
    {
        base.SelectItem(itemDisplay);
        if (InputManager.Instance.NaturalDisasterType_ != itemDisplay.item.item)
        {
            // InputManager.Instance.SelectedTile = null;
        }
        InputManager.Instance.NaturalDisasterType_ = itemDisplay.item.item;
    }
}
