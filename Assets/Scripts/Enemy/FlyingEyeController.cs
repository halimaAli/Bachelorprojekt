using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeController : MonoBehaviour
{
    private Enemy_Movement enemy_Movement;
    public bool playerDetected;

    // Start is called before the first frame update
    void Awake()
    {
        enemy_Movement = GetComponent<Enemy_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            enemy_Movement.MoveLeftandRight();
        } else
        {
            return;
        }
    }
}
