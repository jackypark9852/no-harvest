using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSelectionManager<T> : Singleton<ItemSelectionManager<T>>
{
    // List<T> items;
    public T selectedItem { get; private set; }

    ItemDisplay<T> selectedItemDisplay;
    [SerializeField] ItemDisplay<T> defaultSelectedItemDisplay;
    ItemDisplay<T> prevSelectedItemDisplay;
    
    [SerializeField] Transform itemDisplaysParent;
    protected Dictionary<T, ItemDisplay<T>> itemToDisplayMap = new Dictionary<T, ItemDisplay<T>>();

    protected virtual void Awake()
    {
        InitItemToDisplayMap();
        
        if (defaultSelectedItemDisplay != null)
        {
            SelectItem(defaultSelectedItemDisplay);
        }
    }

    public virtual void SelectItem(ItemDisplay<T> itemDisplay)
    {
        if (selectedItemDisplay is not null)
        {
            DeselectSelectedItem();
        }
        selectedItemDisplay = itemDisplay;
        selectedItem = selectedItemDisplay.item.item;
        selectedItemDisplay.SetFrameActive(true);
        prevSelectedItemDisplay = selectedItemDisplay;
    }

    public void DeselectSelectedItem()
    {
        selectedItem = default;
        if (selectedItemDisplay is null)
        {
            return;
        }
        selectedItemDisplay.SetFrameActive(false);
        selectedItemDisplay = null;
    }

    public virtual void SelectPrevItem()
    {
        if (prevSelectedItemDisplay is null)
        {
            return;
        }
        SelectItem(prevSelectedItemDisplay);
    }
    
    private void InitItemToDisplayMap()
    {
        foreach (ItemDisplay<T> itemDisplay in itemDisplaysParent.GetComponentsInChildren<ItemDisplay<T>>())
        {
            itemToDisplayMap.Add(itemDisplay.item.item, itemDisplay);
        }
    }
}
