using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : CombatController
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleShooting();
        }
    }
}
