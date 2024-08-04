using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] MovingWall wall;
    private bool active;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        active = true;
    }

    internal void Switch()
    {
        if (active)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            wall.Open();
            active = false;
            print("hit");
        }
    }
}
