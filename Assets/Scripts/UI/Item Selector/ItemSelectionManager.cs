using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSelectionManager<T> : Singleton<ItemSelectionManager<T>>
{
    // List<T> items;
    public T selectedItem { get; private set; }

    ItemDisplay<T> selectedItemDisplay;
    [SerializeField] ItemDisplay<T> defaultSelectedItemDisplay;
    // [SerializeField] Transform itemDisplaysParent;

    void Awake()
    {
        if (defaultSelectedItemDisplay != null)
        {
            SelectItem(defaultSelectedItemDisplay);
        }
    }

    public virtual void SelectItem(ItemDisplay<T> itemDisplay)
    {
        if (selectedItemDisplay is not null)
        {
            DeselectItem(selectedItemDisplay);
        }
        selectedItemDisplay = itemDisplay;
        selectedItem = selectedItemDisplay.item.item;
        selectedItemDisplay.SetFrameActive(true);
    }

    public void DeselectItem(ItemDisplay<T> itemDisplay)
    {
        selectedItem = default;
        selectedItemDisplay = null;
        itemDisplay.SetFrameActive(false);
    }
}
