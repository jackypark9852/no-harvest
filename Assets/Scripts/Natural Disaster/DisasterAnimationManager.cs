using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DisasterAnimationManager : Singleton<DisasterAnimationManager>
{
    public List<NaturalDisasterTypeAndPrefab> prefabs; 
    public float tsunamiTravelSpeed;
    public float fireDespawnDelaySeconds = 2f;
    public float meteoriteFallSpeed = 6f;
    public float blizzardFallSpeed = 5f; 
    Dictionary<NaturalDisasterType, GameObject> DisasterTypeToPrefab = new Dictionary<NaturalDisasterType, GameObject>();


    private void Awake()
    {
        prefabs.ForEach((prefab) => DisasterTypeToPrefab.Add(prefab.type, prefab.prefab));
    }
    public async Task PlayDisasterAnimation(NaturalDisasterType naturalDisasterType, List<Tile> affectedTiles)
    {
        switch (naturalDisasterType)
        {
            case NaturalDisasterType.Tsunami:
                await PlayTsunamiAnimation(affectedTiles);
                break;
            case NaturalDisasterType.Fire:
                await PlayFireAnimation(affectedTiles);
                break;
            case NaturalDisasterType.Meteorite:
                await PlayMeteoriteAnimation(affectedTiles);
                break;
            case NaturalDisasterType.Blizzard:
                await PlayBlizzardAnimation(affectedTiles);
                break;
            case NaturalDisasterType.Lightning:
                await PlayLightningAnimation(affectedTiles);
                break; 
            default:
                //throw new System.Exception($"PlayDisasterAnimation: {naturalDisasterType.ToString()} animation not found.");
                Debug.Log("Disaster animation not found");
                break; 
        }
        return;
    }
    public async Task PlayFireAnimation(List<Tile> affectedTiles)
    {
        if (!DisasterTypeToPrefab.ContainsKey(NaturalDisasterType.Fire))
        {
            throw new System.Exception("DisasterAnimationManager: missing \"Fire\" prefab.");
        }
        GameObject firePrefab = DisasterTypeToPrefab[NaturalDisasterType.Fire];
        Vector3 position = new Vector3(TileUtil.GetCenterXCoord(affectedTiles), TileUtil.GetMinYCoord(affectedTiles) - 1, 0);
        Debug.Log(position);
        GameObject fire = Object.Instantiate(firePrefab, position, Quaternion.identity);
        await Task.Delay(Mathf.RoundToInt(fireDespawnDelaySeconds * 1000));
        Destroy(fire); 
        return;
    }
    public async Task PlayTsunamiAnimation(List<Tile> affectedTiles)
    {
        if (!DisasterTypeToPrefab.ContainsKey(NaturalDisasterType.Tsunami))
        {
            throw new System.Exception("DisasterAnimationManager: missing \"Tsunami\" prefab.");
        }
        GameObject tsunamiPrefab = DisasterTypeToPrefab[NaturalDisasterType.Tsunami];

        float xStart = 27;
        float xEnd = -15;
        float yVal = TileUtil.GetMinYCoord(affectedTiles) - 1;
        GameObject tsunami = Object.Instantiate(tsunamiPrefab, new Vector3(xStart, yVal, 0), Quaternion.identity);

        while (tsunami.transform.position.x > xEnd)
        {
            tsunami.transform.position = new Vector3(tsunami.transform.position.x - tsunamiTravelSpeed * Time.deltaTime,
                tsunami.transform.position.y,
                tsunami.transform.position.z);
            await Task.Delay(6);
        }
        Destroy(tsunami);
        return;
    }
    public async Task PlayMeteoriteAnimation(List<Tile> affecetedTiles)
    {
        // Clone affectedTiles
        List<Tile> affectedTilesClone = new List<Tile>(affecetedTiles);
        List<Task> meteoriteDropTasks = new List<Task>();
        if (!DisasterTypeToPrefab.ContainsKey(NaturalDisasterType.Meteorite))
        {
            throw new System.Exception("DisasterAnimationManager: missing \"Tsunami\" prefab.");
        }
        
        foreach(Tile tile in affecetedTiles)
        {
            meteoriteDropTasks.Add(DropMeteorite(tile));
        }
        await Task.WhenAll(meteoriteDropTasks);
        return; 
    }
    public async Task DropMeteorite(Tile tile)
    {
        GameObject meteoritePrefab = DisasterTypeToPrefab[NaturalDisasterType.Meteorite];
        float zStart = -5f;
        float zEnd = 0f;
        Vector2 position = tile.GetCoords(); 
        GameObject meteorite = Object.Instantiate(meteoritePrefab, new Vector3(position.x, position.y, zStart), Quaternion.identity);
        Animator animator = meteorite.GetComponent<Animator>();

        while (meteorite.transform.position.z < zEnd)
        {
            meteorite.transform.position = new Vector3(meteorite.transform.position.x,
                meteorite.transform.position.y,
                meteorite.transform.position.z + meteoriteFallSpeed * Time.deltaTime);
            await Task.Delay(6);
        }
        animator.SetTrigger("Landed"); 
        await Task.Delay(300);
        Destroy(meteorite);
        return;
    }
    public async Task PlayBlizzardAnimation(List<Tile> affecetedTiles)
    {
        // Clone affectedTiles
        List<Tile> affectedTilesClone = new List<Tile>(affecetedTiles);
        List<Task> blizzardDropTasks = new List<Task>();
        if (!DisasterTypeToPrefab.ContainsKey(NaturalDisasterType.Blizzard))
        {
            throw new System.Exception("DisasterAnimationManager: missing \"Blizzard\" prefab.");
        }

        foreach (Tile tile in affecetedTiles)
        {
            blizzardDropTasks.Add(DropBlizzard(tile));
        }
        await Task.WhenAll(blizzardDropTasks);
        return;
    }
    public async Task DropBlizzard(Tile tile)
    {
        GameObject blizzardPrefab = DisasterTypeToPrefab[NaturalDisasterType.Blizzard];
        float zStart = -5f;
        float zEnd = 0f;
        Vector2 position = tile.GetCoords();
        GameObject blizzard = Object.Instantiate(blizzardPrefab, new Vector3(position.x, position.y, zStart), Quaternion.identity);
        Animator animator = blizzard.GetComponent<Animator>();

        while (blizzard.transform.position.z < zEnd)
        {
            blizzard.transform.position = new Vector3(blizzard.transform.position.x,
                blizzard.transform.position.y,
                blizzard.transform.position.z + blizzardFallSpeed * Time.deltaTime);
            await Task.Delay(6);
        }
        animator.SetTrigger("Landed");
        await Task.Delay(300);
        Destroy(blizzard);
        return;
    }
    public async Task PlayLightningAnimation(List<Tile> affectedTiles)
    {
        if (!DisasterTypeToPrefab.ContainsKey(NaturalDisasterType.Lightning))
        {
            throw new System.Exception("DisasterAnimationManager: missing \"Lightning\" prefab.");
        }
        GameObject lightningPrefab = DisasterTypeToPrefab[NaturalDisasterType.Lightning];
        Vector3 position = new Vector3(TileUtil.GetCenterXCoord(affectedTiles), TileUtil.GetCenterYCoord(affectedTiles), 0);
        GameObject lightning = Object.Instantiate(lightningPrefab, position, Quaternion.identity);
        await Task.Delay(700);
        Destroy(lightning);
        return;
    }
}

[System.Serializable]
public struct NaturalDisasterTypeAndPrefab
{
    public NaturalDisasterType type;
    public GameObject prefab; 
}
