using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "ScriptableObjects/PlantData", order = 1)]

public class PlantData : ScriptableObject
{
    [SerializeField]
    public List<Tuple<PlantType, GameObject>> plantsList = new List<Tuple<PlantType, GameObject>>();
}

[Serializable]
public struct Tuple<T1, T2>
{
    public T1 Item1;
    public T2 Item2;

    public Tuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}
