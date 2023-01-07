using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlantUtil : MonoBehaviour
{
    PlantData plantData;
    public Dictionary<PlantType, GameObject> plantsDict = new Dictionary<PlantType, GameObject>();

    private void Awake()
    {
        SetUpDictionary(); 
    }

    public GameObject PlantTypeToPrefab(PlantType plantType) {
        if (plantsDict.ContainsKey(plantType))
        {
            return plantsDict[plantType];
        }
        else
        {
            throw new ArgumentException("Plant type not found");
        }
    }
    

    void SetUpDictionary()
    {
        plantData.plantsList.ForEach(x => plantsDict.Add(x.Item1, x.Item2));
    }
}
