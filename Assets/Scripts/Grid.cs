using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Tile[] tiles;

    void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
    }
}
