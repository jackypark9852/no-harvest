using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DisasterAnimationManager : Singleton<DisasterAnimationManager>
{
    public GameObject tsunamiPrefab;
    public float tsunamiTravelSpeed; 

    public async Task PlayDisasterAnimation(NaturalDisasterType naturalDisasterType, List<Tile> affectedTiles)
    {
        switch(naturalDisasterType)
        {
            case NaturalDisasterType.Tsunami:
                await PlayTsunamiAnimation(affectedTiles);
                break;
            default:
                throw new System.Exception($"PlayDisasterAnimation: {naturalDisasterType.ToString()} animation not found.");
        }
        return;
    }
    public async Task PlayTsunamiAnimation(List<Tile> affectedTiles)
    {

        float xStart = 27;
        float xEnd = -15;
        if (tsunamiPrefab is null)
        {
            throw new System.Exception("DisasterAnimationManager: missing \"Tsunami\" prefab.");
        }

        int yVal = int.MaxValue; 
        foreach(Tile tile in affectedTiles)
        {
            yVal = Mathf.Min(yVal, tile.GetCoords().y - 1);
        }

        GameObject tsunami = Object.Instantiate(tsunamiPrefab, new Vector3(xStart, (float)yVal, 0), Quaternion.identity);

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
