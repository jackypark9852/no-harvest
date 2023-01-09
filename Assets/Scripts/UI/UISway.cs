using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISway : MonoBehaviour
{
    public float swayAmountX;
    public float swayAmountY; 
    public float swayPeriodX;
    public float swayPeriodY; 
    float elapsedTime;
    Vector3 startPosition; 
    
    public float rotationSpeed = 1f;  //Speed variable used to control the animation
    public float rotationOffset = 50f; //Rotate by 50 units
    float finalAngle;  //Keeping track of final angle to keep code cleaner
    Vector3 startAngle;   //Reference to the object's original angle values

    private void Awake()
    {
        elapsedTime = 0f;
        startAngle = transform.rotation.eulerAngles;  // Get the start position
        startPosition = transform.position;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateUIPosition(); 
    }

    // Update is called once per frame
    void UpdateUIPosition()
    {
        float deltaX = Mathf.Sin(elapsedTime / swayPeriodX) * swayAmountX;
        float deltaY = Mathf.Sin(elapsedTime / swayPeriodY) * swayAmountY;
        transform.position = startPosition + new Vector3(deltaX, deltaY, 0f);

        finalAngle = startAngle.z + (float)Math.Sin(elapsedTime * rotationSpeed) * rotationOffset;  //Calculate animation angle
        transform.eulerAngles = new Vector3(startAngle.x, startAngle.y, finalAngle); //Apply new angle to object
    }
}

