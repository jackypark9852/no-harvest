using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TileName : MonoBehaviour
{
    Vector2Int coords;

    void Update()
    {
        coords.x = Mathf.RoundToInt(transform.position.x / transform.localScale.x);
        coords.y = Mathf.RoundToInt(transform.position.y / transform.localScale.y);
        name = coords.ToString();
    }
}
