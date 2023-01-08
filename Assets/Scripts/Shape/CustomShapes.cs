using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomShapes
{
    public static List<Vector2Int> DIRECTIONS_4 = new List<Vector2Int>()
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
    };

    public static ShapeData CreateBFSShapeData(Vector2Int startingCoords, int radius)
    {
        List<Vector2Int> affectedTiles = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startingCoords);
        while (queue.Count > 0)
        {
            Vector2Int currentCoords = queue.Dequeue();
            affectedTiles.Add(currentCoords);
            if (affectedTiles.Count > radius)
            {
                break;
            }
            foreach (Vector2Int direction in DIRECTIONS_4)
            {
                Vector2Int newCoords = currentCoords + direction;
                if (!affectedTiles.Contains(newCoords))
                {
                    queue.Enqueue(newCoords);
                }
            }
        }
        return new ShapeData(ShapeType.Custom, affectedTiles.ToArray());
    }
    /*
    {
        ShapeData shapeData = ScriptableObject.CreateInstance<ShapeData>();
        shapeData.shapeType = ShapeType.Custom;
        return shapeData;
    }
    */
}
