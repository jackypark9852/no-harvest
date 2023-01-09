using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyOnAnimationComplete : MonoBehaviour
{
    [SerializeField] float delay = 0;
    [SerializeField] bool destroyParent = false;

    void Start()
    {
        Debug.Log("here");
        float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        GameObject gameObjectToDestroy = destroyParent ? transform.parent.gameObject : gameObject;
        Destroy(gameObjectToDestroy, animTime + delay);
    }
}
