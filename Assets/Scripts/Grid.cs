using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Tile[] tiles;
    public Tile[] Tiles
    {
       get { return tiles; }
    }

    void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
    }
}
