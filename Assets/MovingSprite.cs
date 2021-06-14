using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSprite : MonoBehaviour
{
    public float ScrollY, ScrollX;

    private void Update()
    {
        float offsetX = ScrollX * Time.deltaTime;
        float offsetY = ScrollY * Time.deltaTime;

    }
}
