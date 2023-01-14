using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] UnityEvent OnScoreChange;
    [SerializeField] UnityEvent OnComboChange;

    public int score { get; private set; } = 0;

    public float comboMultiplier { get; private set; } = 1f;
    [SerializeField] float comboMin = 1;
    [SerializeField] float comboMax = 5;
    [SerializeField] float comboIncrement = 0.5f;
    [SerializeField] float minProportionToIncrementCombo = 0.7f;

    [SerializeField] int plantDestroyedScore = 100;
    [SerializeField]
    Dictionary<float, int> comboProportionScores = new Dictionary<float, int>
    {
        { 1f, 500 },
        { 0.7f, 300 },
        { 0f, 0 },
    };

    private void IncreaseScoreByRawAmount(int incAmt)
    {
        int prevScore = score;
        score += Mathf.RoundToInt(incAmt * comboMultiplier);
        if (score != prevScore)
        {
            OnScoreChange.Invoke();
        }
    }

    private void UpdateCombo(float plantsDestroyedProportion)
    {
        float prevComboMultiplier = comboMultiplier;
        if (isGreaterThanOrAlmostEqual(plantsDestroyedProportion, minProportionToIncrementCombo))
        {
            comboMultiplier += comboIncrement;
            comboMultiplier = Mathf.Min(comboMultiplier, comboMax);
        }
        else
        {
            comboMultiplier = comboMin;
        }
        if (comboMultiplier != prevComboMultiplier)
        {
            OnComboChange.Invoke();
        }
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

    public void IncreaseScoreFromSinglePlant(TileInput.EffectType effectType)
    {
        switch (effectType)
        {
            case TileInput.EffectType.Destroyed:
                IncreaseScoreByRawAmount(plantDestroyedScore);
                return;
            default:
                return;
        }
    }
}