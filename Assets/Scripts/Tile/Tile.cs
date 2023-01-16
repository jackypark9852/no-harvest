using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Plant plant = null;
    private PlantAnimation plantAnimation = null; 
    public Plant Plant
    {
        get { return plant; }
        private set {
            if (plant is not null)
            {
                throw new System.Exception("Tile already has a plant");
            } else if (plantable == false)
            {
                throw new System.Exception("Tile is not plantable");
            }
            plant = value;
        }
    }
    public PlantAnimation PlantAnimation
    {
        get { return plantAnimation; }
        private set {
            if (plant is not null)
            {
                throw new System.Exception("Tile already has a plant");
            } else if (plantable == false)
            {
                throw new System.Exception("Tile is not plantable");
            }
            plantAnimation = value;
        }
    }
    
    public bool plantable = true;

    public Vector2Int GetCoords()
    {
        int coords_x = Mathf.RoundToInt(transform.position.x / transform.localScale.x);
        int coords_y = Mathf.RoundToInt(transform.position.y / transform.localScale.y);
        return new Vector2Int(coords_x, coords_y);
    }

    public void PlantNewPlant(PlantType plantType)
    {
        if (plant is not null)
        {
            return;
        }
        GameObject plantPrefab = PlantUtil.Instance.PlantTypeToPrefab(plantType);
        GameObject plantObject = Object.Instantiate(plantPrefab, transform.position, Quaternion.identity); 
        plant = plantObject.GetComponent<Plant>(); 
        plantAnimation = plantObject.GetComponent<PlantAnimation>(); 
        plant.gameObject.transform.parent = gameObject.transform; 
        plant.PlantDestroyed.AddListener(OnPlantDestroyed);
    }

    public void OnPlantDestroyed()
    {
        plant = null; 
    }
}
