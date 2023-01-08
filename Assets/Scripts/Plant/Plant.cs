using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Plant : MonoBehaviour
{
    public UnityEvent PlantDestroyed; 
    protected abstract PlantType plantType { get; set; }
    public virtual TileInput.EffectType GetEffectType(NaturalDisasterType naturalDisasterType) {
        return TileInput.EffectType.Growth;
    }

    public abstract TileInput.EffectType OnNaturalDisaster(NaturalDisasterType naturalDisasterType);

    public virtual void OnPlantDestroyed()
    {
        PlantDestroyed.Invoke(); 
        Destroy(gameObject);
    }
}
