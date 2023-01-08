using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlaySpawnAnimation()
    {
        animator.SetTrigger(PlantAnimationTrigger.Spawn.ToString());
    }
}
