using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DisasterAnimationManager : Singleton<DisasterAnimationManager>
{
    public List<NaturalDisasterTypeAndPrefab> prefabs; 
    public float tsunamiTravelSpeed;
    public float fireDespawnDelaySeconds = 2f;
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
            default:
                throw new System.Exception($"PlayDisasterAnimation: {naturalDisasterType.ToString()} animation not found.");
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
}

[System.Serializable]
public struct NaturalDisasterTypeAndPrefab
{
    public NaturalDisasterType type;
    public GameObject prefab; 
}
