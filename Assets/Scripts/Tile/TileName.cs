using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileName : MonoBehaviour
{
    Tile tile;

    void Start()
    {
        tile = GetComponent<Tile>();
    }

    void Update()
    {
        name = tile.GetCoords().ToString();
    }
}
