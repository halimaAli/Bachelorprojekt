using UnityEngine;

public class PlayerCombat : CombatController
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootProjectile();
        }
    }
}
