using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class Plant : MonoBehaviour
{
    public UnityEvent PlantDestroyed;
    public UnityEvent PlantCreated; 
    protected abstract PlantType plantType { get; set; }
    public abstract TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType); 

    public virtual TileInput.EffectType OnNaturalDisaster(NaturalDisasterType naturalDisasterType)
    {
        TileInput.EffectType effectType = GetEffectType(naturalDisasterType);
        if (effectType == TileInput.EffectType.Destroyed)
        {
            OnPlantDestroyed();
        }
        return effectType;
    }

    public virtual async void OnPlantDestroyed()
    {
        PlantDestroyed.Invoke();
        await Task.Delay(1000);
        Destroy(gameObject);
    }
}
