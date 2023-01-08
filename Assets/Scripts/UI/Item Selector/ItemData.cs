using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData<T> : ScriptableObject
{
    public T item;
    // public string itemName;
    public Sprite sprite;
}
