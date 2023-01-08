using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSelectionManager<T> : Singleton<ItemSelectionManager<T>>
{
    [SerializeField] GameObject itemsParent;
    List<T> items;
    public T selectedItem { get; private set; }

    void Awake()
    {
        items = GetComponentsInChildren<T>(itemsParent);
    }

    public void SelectItem(T item)
    {
        selectedItem = item;
    }
}
