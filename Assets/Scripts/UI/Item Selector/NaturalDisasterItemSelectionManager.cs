using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalDisasterItemSelectionManager : ItemSelectionManager<NaturalDisasterType>
{
    public override void SelectItem(ItemDisplay<NaturalDisasterType> itemDisplay)
    {
        base.SelectItem(itemDisplay);
        InputManager.Instance.naturalDisasterType = itemDisplay.item.item;
    }
}
