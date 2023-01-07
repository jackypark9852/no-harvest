using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInput : MonoBehaviour
{
    EffectType effectType = EffectType.NONE;
    bool isBlinking = false;

    [SerializeField] float alphaMinMultiplier = 0.25f;
    [SerializeField] float alphaMaxMultiplier = 1.25f;
    [SerializeField] float blinkPeriod = 1f;

    public enum EffectType
    {
        NONE,
        DESTROYED,
        GROWTH,
        NEUTRAL,
    }

    Dictionary<EffectType, Color> effectToColor = new Dictionary<EffectType, Color> {
        [EffectType.NONE] = Color.clear,
        [EffectType.DESTROYED] = new Color(1f, 0f, 0f, 0.5f),
        [EffectType.GROWTH] = new Color(0f, 1f, 0f, 0.5f),
        [EffectType.NEUTRAL] = new Color(0.5f, 0.5f, 0.5f, 0.5f),
    };

    [SerializeField] SpriteRenderer spriteRenderer;

    void Update()
    {
        if (isBlinking)
        {
            float alpha = Mathf.Lerp(alphaMinMultiplier * effectToColor[effectType].a, alphaMaxMultiplier * effectToColor[effectType].a, (Mathf.Sin(Time.time * (Mathf.PI / blinkPeriod)) + 1) / 2f);
            Color baseColor = effectToColor[effectType];
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        }
        else
        {
            spriteRenderer.color = effectToColor[effectType];
        }
    }

    void OnMouseDown()
    {
        effectType = EffectType.DESTROYED;
        isBlinking = true;
    }
}
