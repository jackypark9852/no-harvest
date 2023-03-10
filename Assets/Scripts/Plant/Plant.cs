using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class Plant : MonoBehaviour
{
    public UnityEvent PlantDestroyed;
    public UnityEvent PlantDespawned; 
    public UnityEvent PlantCreated;
    public UnityEvent PlantImmune;
    protected abstract PlantType plantType { get; set; }

    ShapeData squareMediumShapeData;
    ShapeData circleSmallShapeData;

    void Awake()
    {
        squareMediumShapeData = CreateSquareMediumShapeData();
        circleSmallShapeData = CreateCircleSmallShapeData();
    }

    private ShapeData CreateSquareMediumShapeData()
    {
        ShapeData shapeData = ScriptableObject.CreateInstance<ShapeData>();
        shapeData.shapeType = ShapeType.SquareMedium;
        shapeData.affectedTiles = new Vector2Int[] { new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1) };
        return shapeData;
    }

    private ShapeData CreateCircleSmallShapeData()
    {
        ShapeData shapeData = ScriptableObject.CreateInstance<ShapeData>();
        shapeData.shapeType = ShapeType.SquareMedium;
        shapeData.affectedTiles = new Vector2Int[] { new Vector2Int(0, -1), new Vector2Int(-1, 0), /* new Vector2Int(0, 0), */ new Vector2Int(1, 0), new Vector2Int(0, 1) };
        return shapeData;
    }

    public abstract TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType); 

    public virtual TileInput.EffectType OnNaturalDisaster(NaturalDisasterType naturalDisasterType)
    {
        TileInput.EffectType effectType = GetEffectType(naturalDisasterType);
        if (effectType == TileInput.EffectType.Destroyed)
        {
            OnPlantDestroyed();
        }
        else if (effectType == TileInput.EffectType.Growth)
        {
            OnPlantGrowth();
        }
        return effectType;
    }

    public virtual async void OnPlantDestroyed()
    {
        PlantDestroyed.Invoke();
        await UniTask.Delay(1000);
        Destroy(gameObject);
    }

    public virtual void OnPlantGrowth()
    {
        PlantImmune.Invoke();
        Vector2Int coords = GetTileCoords();
        List<Tile> affectedTiles = TileUtil.GetAffectedTiles(coords, circleSmallShapeData);
        
        // Hard-coded
        System.Random rnd = new System.Random();
        var affectedTiles2 = affectedTiles.OrderBy(x => rnd.Next()).Take(2);

        foreach (Tile tile in affectedTiles2)  // Hard-coded
        {
            if (tile.Plant is null)
            {
                tile.PlantNewPlant(plantType);
            }
        }
    }

    public Vector2Int GetTileCoords()
    {
        Tile tile = GetComponentInParent<Tile>();
        return tile.GetCoords();
    }
}
