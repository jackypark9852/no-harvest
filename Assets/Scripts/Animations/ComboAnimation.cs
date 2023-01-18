using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ComboAnimation : MonoBehaviour
{
    [Header("Combo Text (near disaster)")]
    [SerializeField] Grid grid;
    [SerializeField] GameObject comboSpritePrefab;  
    [SerializeField] float comboSpriteXOffset = 0f;
    [SerializeField] float comboSpriteYOffset = 1f;

    [Header("Combo Text (top right)")]
    [SerializeField] TMP_Text comboText;
    [SerializeField] Color blinkColor = Color.green;
    [SerializeField] int blinkCount = 2;
    [SerializeField] int blinkIntervalMillieSecond = 150;

    private void Awake() {
        GameObject gridObj = GameObject.FindWithTag("Grid");
        grid = gridObj.GetComponent<Grid>(); // TODO: Find a better way to do this
    }

    public void Play() {
        Vector3 comboSpritePos = grid.destroyedPlantsCenter + new Vector3(comboSpriteXOffset, comboSpriteYOffset);
        GameObject comboSpriteObj = Instantiate(comboSpritePrefab, comboSpritePos, comboSpritePrefab.transform.rotation);
        TMPro.TextMeshPro text = comboSpriteObj.GetComponentInChildren<TMPro.TextMeshPro>();
        string comboMultiplierText;
        if (ScoreManager.Instance.WasComboPreviouslyMaxed())
        {
            comboMultiplierText = "COMBO\nMAXED!";
        }
        else
        {
            comboMultiplierText = "COMBO\nUP!";
            // comboMultiplierText = $"COMBO {ScoreManager.Instance.comboMultiplier.ToString()}X";
        }
        text.text = comboMultiplierText;

        PlayComboTextBlinks();
    }

    async void PlayComboTextBlinks()
    {
        Color original = comboText.color;
        for (int i = 0; i < blinkCount; i++)
        {
            comboText.color = blinkColor;
            await UniTask.Delay(blinkIntervalMillieSecond);
            comboText.color = original;
            await UniTask.Delay(blinkIntervalMillieSecond / 2);
        }
    }
}
