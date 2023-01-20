using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrollAreaDebug : MonoBehaviour
{
    public void OnScrollAreaChanged(Vector2 value)
    {
        Debug.Log("ScrollAreaDebug: OnScrollAreaChanged: " + value);
    }
}
