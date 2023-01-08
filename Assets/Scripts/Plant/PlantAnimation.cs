using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.ShaderGraph.Internal;
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
    Vector3 targetVector;
    public float swayPeriod = 1.0f;
    public float swayAmount = 50f;
    float elapsedTime = 0.0f;

    public Material blinkMaterial;
    public int blinkCount = 3;
    public int blinkIntervalMillieSecond = 50; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        targetVector = this.transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(targetVector, Camera.main.transform.rotation * Vector3.up);
        startingRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; 
        float sway = Mathf.Sin(elapsedTime * Mathf.PI * 2 / swayPeriod) * swayAmount;
        transform.eulerAngles = new Vector3(startingRotation.x, startingRotation.y, startingRotation.z + sway);
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
            Debug.Log("Blink"); 
            spriteRenderer.material = blinkMaterial;
            await Task.Delay(blinkIntervalMillieSecond);
            spriteRenderer.material = original;
            await Task.Delay(blinkIntervalMillieSecond); 
        }
    }
}
