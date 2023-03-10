using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] UnityEvent OnScoreChange;
    [SerializeField] UnityEvent OnComboChange;
    [SerializeField] UnityEvent OnComboIncrease;

    public int score { get; private set; } = 0;

    public float comboMultiplier { get; private set; } = 1f;
    [SerializeField] float comboMin = 1;
    [SerializeField] float comboMax = 3;
    Dictionary<float, float> comboProportionComboIncrements = new Dictionary<float, float>
    {
        { 1f, 0.5f },
        { 0.7f, 0.25f },
        { 0f, -0.5f },
    };
    [SerializeField] int plantDestroyedScore = 100;
    Dictionary<float, int> comboProportionScores = new Dictionary<float, int>
    {
        // { 1f, 500 },
        // { 0.7f, 300 },
        { 0f, 0 },
    };

    private float prevComboMultiplier;

    public ComboAnimation comboAnimation { get; private set; }
    private void Awake()
    {
        comboAnimation = GetComponent<ComboAnimation>();
        prevComboMultiplier = comboMultiplier;
    }

    private int IncreaseScoreByRawAmount(int incAmt)
    {
        int prevScore = score;
        int incAmtWithCombo = Mathf.RoundToInt(incAmt * comboMultiplier);
        score += incAmtWithCombo;
        if (score != prevScore)
        {
            OnScoreChange.Invoke();
        }
        return incAmtWithCombo;
    }

    private void UpdateCombo(float plantsDestroyedProportion)
    {
        float prevComboMultiplier = comboMultiplier;

        float closestKey = comboProportionComboIncrements.Keys.Where(x => isGreaterThanOrAlmostEqual(plantsDestroyedProportion, x)).Max();
        comboMultiplier += comboProportionComboIncrements[closestKey];
        if (comboProportionComboIncrements[closestKey] > 0f)
        {
            comboMultiplier = Mathf.Min(comboMultiplier, comboMax);
            OnComboIncrease.Invoke();
        }
        else
        {
            comboMultiplier = Mathf.Max(comboMultiplier, comboMin);
        }

        if (comboMultiplier != prevComboMultiplier)
        {
            OnComboChange.Invoke();
        }
        this.prevComboMultiplier = comboMultiplier;
    }

    public void IncreaseScoreFromComboAndUpdateCombo(int plantsDestroyedCount, int disasterTileCount)
    {
        float plantsDestroyedProportion = (float)plantsDestroyedCount / disasterTileCount;
        IncreaseScoreByRawAmount(GetComboProportionScore(plantsDestroyedProportion));
        UpdateCombo(plantsDestroyedProportion);
    }

    public int GetComboProportionScore(float plantsDestroyedProportion)
    {
        float closestKey = comboProportionScores.Keys.Where(x => isGreaterThanOrAlmostEqual(plantsDestroyedProportion, x)).Max();

        return comboProportionScores[closestKey];
    }

    private bool isGreaterThanOrAlmostEqual(float a, float b, float tolerance = 0.0001f)
    {
        return a > b - tolerance;
    }

    public int IncreaseScoreFromSinglePlant(TileInput.EffectType effectType)
    {
        switch (effectType)
        {
            case TileInput.EffectType.Destroyed:
                return IncreaseScoreByRawAmount(plantDestroyedScore);
            default:
                return 0;
        }
    }

    public bool WasComboPreviouslyMaxed()
    {
        return prevComboMultiplier == comboMax;
    }
}
