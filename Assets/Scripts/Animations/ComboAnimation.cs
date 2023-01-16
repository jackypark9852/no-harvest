using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAnimation : MonoBehaviour
{
    [SerializeField] GameObject comboSpritePrefab;  
    public void Play(Vector3 position, float comboMultiplier) {
        GameObject comboSpriteObj = Instantiate(comboSpritePrefab, position, comboSpritePrefab.transform.rotation);
        TMPro.TextMeshPro text = comboSpriteObj.GetComponentInChildren<TMPro.TextMeshPro>(); 
        text.text = $"COMBO {comboMultiplier.ToString()}X"; 
    }
}
