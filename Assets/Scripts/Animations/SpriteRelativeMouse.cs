using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRelativeMouse : MonoBehaviour
{
    public float moveAmount = 1.0f;
    public float moveSpeed = 1.0f; 
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        // Get the current mouse position
        Vector3 mousePos = Input.mousePosition;

        // Convert the mouse position to world coordinates
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Calculate the difference between the world position and the sprite's position
        Vector3 diff = worldPos - transform.position;
        diff = new Vector3(diff.x, diff.y, 0f);

        Vector3 targetPosition = startPosition - diff.normalized * moveAmount;

        // Move the sprite in the opposite direction of the mouse
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
