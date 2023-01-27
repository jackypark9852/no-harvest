using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Grid : MonoBehaviour
{ 
    Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Tile> Tiles
    {
        get { return tiles; }
    }
    Dictionary<Vector2Int, TileInput> tileInputs = new Dictionary<Vector2Int, TileInput>();
    public Vector3 destroyedPlantsCenter {get; private set;}
    void Awake()
    {
        Tile[] tiles1D = GetComponentsInChildren<Tile>();

        foreach (Tile tile in tiles1D)
        {
            tiles[tile.GetCoords()] = tile;
            tileInputs[tile.GetCoords()] = tile.GetComponent<TileInput>();
        }
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[new Vector2Int(x, y)];
    }

    public List<Tile> GetTiles()
    {
        return tiles.Values.ToList();
    }

    public TileInput GetTileInput(Tile tile)
    {
        if (tile is null)
        {
            return null;
        }
        return tileInputs[tile.GetCoords()];
    }

    // TODO: might overwrite tile highlights incorrectly, fix later
    public void ApplyConfirmedActionsOnTiles(List<PlayerActionInfo> confirmedActions)
    {
        foreach (Tile tile in GetTiles())
        {
            GetTileInput(tile).ResetEffectType();
        }
        foreach (PlayerActionInfo confirmedAction in confirmedActions)
        {
            ApplyConfirmedActionOnTiles(confirmedAction);
        }
    }

    public void ApplyConfirmedActionOnTiles(PlayerActionInfo confirmedAction)
    {
        NaturalDisasterData naturalDisasterData = NaturalDisasterUtil.Instance.NaturalDisasterTypeToData[confirmedAction.naturalDisasterType];
        Vector2Int centerTileCoordinate = confirmedAction.centerTileCoordinate;
        ShapeData shapeData = naturalDisasterData.shapeData;

        List<Tile> affectedTiles = TileUtil.GetAffectedTiles(centerTileCoordinate, shapeData);
        foreach (Tile tile in affectedTiles)
        {
            TileInput tileInput = GetTileInput(tile);
            if (tile.Plant)
            {
                tileInput.effectType = tile.Plant.GetEffectType(confirmedAction.naturalDisasterType);
            }
            else
            {
                tileInput.effectType = TileInput.EffectType.Destroyed;
            }
            tileInput.isBlinking = confirmedAction.actionInputType == ActionInputType.Hovered;
        }
    }

    public async void ApplyFarmerActionOnTiles()
    {
        List<FarmerActionInfo> farmerActionInfos = GameManager.Instance.farmerActionInfos;
        List<UniTask> plantTasks = new List<UniTask>();
        
        foreach (FarmerActionInfo actionInfo in farmerActionInfos)
        {
            Vector2Int centerTile = actionInfo.centerTileCoordinate;
            ShapeData shapeData = actionInfo.shapeData;
            PlantType plantType = actionInfo.plantType;

            List<Tile> affectedTiles = TileUtil.GetAffectedTiles(centerTile, shapeData);
            
            foreach (Tile tile in affectedTiles)
            {
                if (tile.plantable && tile.Plant == null)
                {
                    int delayMs = UnityEngine.Random.Range(2000, 3000);
                    plantTasks.Add(Plant(tile, plantType, delayMs));
                }
            }
        }

        await UniTask.WhenAll(plantTasks);

        GameManager.Instance.EndFarming();
    }

    public async UniTask PlantOnEveryTile()
    {
        List<UniTask> plantTasks = new List<UniTask>();
        foreach (Tile tile in GetTiles())
        {
            if (tile.plantable && tile.Plant == null)
            {
                int delayMs = UnityEngine.Random.Range(400, 1000);
                PlantType randomPlant = (PlantType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(PlantType)).Length);
                plantTasks.Add(Plant(tile, randomPlant, delayMs));
            }
        }

        await UniTask.WhenAll(plantTasks);
    }
    
    private async UniTask Plant(Tile tile, PlantType plantType, int delayMillieSeconds) 
    {
        await UniTask.Delay(delayMillieSeconds); 
        tile.PlantNewPlant(plantType);
    }


    public async void ApplyPlayerActionOnTiles()
    {
        List<PlayerActionInfo> playerActionInfos = new List<PlayerActionInfo>(GameManager.Instance.playerActionInfos);
        foreach (PlayerActionInfo actionInfo in playerActionInfos)
        {
            Vector2Int centerTileCoordinate = actionInfo.centerTileCoordinate;
            NaturalDisasterType naturalDisasterType = actionInfo.naturalDisasterType;
            ActionInputType actionInputType = actionInfo.actionInputType;

            // Don't execute actions that are "hover" or "selected" 
            if (actionInputType == ActionInputType.Confirmed)  // Possibly not necessary
            {
                if (NaturalDisasterUtil.Instance.NaturalDisasterTypeToData.ContainsKey(naturalDisasterType)) {
                    ShapeData shapeData = NaturalDisasterUtil.Instance.NaturalDisasterTypeToData[naturalDisasterType].shapeData;
                    List<Tile> affectedTiles = TileUtil.GetAffectedTiles(centerTileCoordinate, shapeData);
                    List<Tile> tilesWithPlant = affectedTiles.Where(tile => tile.Plant != null).ToList();
                    List<Tile> tilesWithDestroyedPlant = new List<Tile>();
                    await DisasterAnimationManager.Instance.PlayDisasterAnimation(naturalDisasterType, affectedTiles);

                    int destroyedPlantsCount = 0;
                    foreach (Tile tile in tilesWithPlant)
                    {
                        if (tile.Plant is not null)
                        {
                            TileInput.EffectType effectType = tile.Plant.OnNaturalDisaster(naturalDisasterType);
                            int plantScore = ScoreManager.Instance.IncreaseScoreFromSinglePlant(effectType);  
                            tile.PlantAnimation.SetScore(plantScore); // For visual effect, make score earned appear on top of plant 
                            if (effectType == TileInput.EffectType.Destroyed)
                            {
                                tilesWithDestroyedPlant.Add(tile);
                                destroyedPlantsCount++;
                            }
                        }
                    }
                    destroyedPlantsCenter = new Vector3(TileUtil.GetCenterXCoord(tilesWithDestroyedPlant), TileUtil.GetCenterYCoord(tilesWithDestroyedPlant), 0);
                    ScoreManager.Instance.IncreaseScoreFromComboAndUpdateCombo(destroyedPlantsCount, shapeData.affectedTiles.Length);
                } else
                {
                    throw new Exception($"NaturalDisasterType not found: {naturalDisasterType.ToString()}"); 
                }
            }
        }
        GameManager.Instance.EndDestroying(); 
    }
}
