using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInput : MonoBehaviour
{
    public EffectType effectType { get; set; } = EffectType.None;
    public bool isBlinking { get; set; } = false;
    public bool isHovered { get; set; } = false;
    public bool isSelected { get; set; } = false;

    [SerializeField] float alphaMinMultiplier = 0.25f;
    [SerializeField] float alphaMaxMultiplier = 1.25f;
    [SerializeField] float blinkPeriod = 1f;

    public enum EffectType
    {
        None,
        Destroyed,
        Growth,
        Neutral,
    }

    public enum InputType
    {
        None,
        HoveredButNotSelected,
        Selected,
    }

    public Dictionary<EffectType, Color> effectToColor { get; private set; } = new Dictionary<EffectType, Color> {
        [EffectType.None] = Color.clear,
        [EffectType.Destroyed] = new Color(1f, 0f, 0f, 0.5f),
        [EffectType.Growth] = new Color(0f, 1f, 0f, 0.5f),
        [EffectType.Neutral] = new Color(0.5f, 0.5f, 0.5f, 0.5f),
    };
    
    public Dictionary<InputType, Color> inputToColor { get; private set; } = new Dictionary<InputType, Color> {
        [InputType.None] = Color.clear,
        [InputType.HoveredButNotSelected] = new Color(0.5f, 0.5f, 0.5f, 0.3f),
        [InputType.Selected] = new Color(0.5f, 0.5f, 0.5f, 0.6f),
    };

    [SerializeField] SpriteRenderer effectSpriteRenderer;
    [SerializeField] SpriteRenderer inputSpriteRenderer;
    Tile tile;

    void Awake()
    {
        tile = GetComponent<Tile>();
    }

    void Update()
    {
        if (isBlinking)
        {
            float alpha = Mathf.Lerp(alphaMinMultiplier * effectToColor[effectType].a, alphaMaxMultiplier * effectToColor[effectType].a, (Mathf.Sin(Time.time * (Mathf.PI / blinkPeriod)) + 1) / 2f);
            Color baseColor = effectToColor[effectType];
            effectSpriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        }
        else
        {
            effectSpriteRenderer.color = effectToColor[effectType];
        }
        inputSpriteRenderer.color = inputToColor[GetInputType()];
    }

    private InputType GetInputType()
    {
        if (isSelected)
        {
            return InputType.Selected;
        }
        if (isHovered)
        {
            return InputType.HoveredButNotSelected;
        }
        return InputType.None;
    }

    void OnMouseDown()
    {
        InputManager.Instance.SelectedTile = tile;
        /*
        effectType = EffectType.Destroyed;
        isBlinking = true;
        */
    }

    void OnMouseEnter()
    {
        InputManager.Instance.HoveredTile = tile;
    }
}
