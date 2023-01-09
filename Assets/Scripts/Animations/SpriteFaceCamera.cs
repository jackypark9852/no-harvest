using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFaceCamera : MonoBehaviour
{
    Vector3 targetVector;
    // Start is called before the first frame update
    void Start()
    {
        targetVector = this.transform.position - Camera.main.transform.position;
        float rotationX = Quaternion.LookRotation(targetVector, Camera.main.transform.rotation * Vector3.up).x;
        transform.Rotate(-35f, 0, 0); 
    }
}

