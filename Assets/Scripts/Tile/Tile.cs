using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Plant plant { get; private set; } = null;
    public bool plantable = true;

    public Vector2Int GetCoords()
    {
        int coords_x = Mathf.RoundToInt(transform.position.x / transform.localScale.x);
        int coords_y = Mathf.RoundToInt(transform.position.y / transform.localScale.y);
        return new Vector2Int(coords_x, coords_y);
    }
}
