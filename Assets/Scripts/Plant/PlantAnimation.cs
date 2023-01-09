using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
// using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum PlantAnimationTrigger
{
    Spawn, 
    Idle, 
    Destroyed, 
}

public class PlantAnimation : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer; 

    Vector3 startingRotation;
    public float swayPeriod = 1.0f;
    public float swayAmount = 50f;
    float elapsedTime = 0.0f;

    public Material blinkMaterial;
    public int blinkCount = 2;
    public int blinkIntervalMillieSecond = 50;
    public float startingRotationZ;

    [SerializeField] GameObject immuneSpriteGO;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; 
        float sway = Mathf.Sin(elapsedTime * Mathf.PI * 2 / swayPeriod) * swayAmount;
        float x = transform.rotation.eulerAngles.x;
        float y = transform.rotation.eulerAngles.y; 
        transform.eulerAngles = new Vector3(x, y, startingRotationZ + sway);
    }

    public void PlaySpawnAnimation()
    {
        animator.SetTrigger(PlantAnimationTrigger.Spawn.ToString());
    }

    public void PlayDeathAnimation() 
    {
        PlayDeathBlinks();
    }

    async void PlayDeathBlinks()
    {
        Material original = spriteRenderer.material;
        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.material = blinkMaterial;
            await Task.Delay(blinkIntervalMillieSecond);
            spriteRenderer.material = original;
            await Task.Delay(blinkIntervalMillieSecond); 
        }
    }

    public void InstantiateImmuneSprite()
    {
        Instantiate(immuneSpriteGO, transform.position, immuneSpriteGO.transform.rotation);
    }
}
