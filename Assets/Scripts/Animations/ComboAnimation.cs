using UnityEngine;

public class ComboAnimation : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] GameObject comboSpritePrefab;  
    [SerializeField] float comboSpriteXOffset = 0f;
    [SerializeField] float comboSpriteYOffset = 1f;

    private void Awake() {
        GameObject gridObj = GameObject.FindWithTag("Grid");
        grid = gridObj.GetComponent<Grid>(); // TODO: Find a better way to do this
    }

    public void Play() {
        Vector3 comboSpritePos = grid.destroyedPlantsCenter + new Vector3(comboSpriteXOffset, comboSpriteYOffset);
        GameObject comboSpriteObj = Instantiate(comboSpritePrefab, comboSpritePos, comboSpritePrefab.transform.rotation);
        TMPro.TextMeshPro text = comboSpriteObj.GetComponentInChildren<TMPro.TextMeshPro>(); 
        text.text = $"COMBO {ScoreManager.Instance.comboMultiplier.ToString()}X"; 
    }
}
