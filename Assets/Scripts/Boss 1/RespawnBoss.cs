using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBoss : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        print("entered");
        if (other.gameObject.tag.Equals("Boss"))
        {
            print("respawn");
            BossStateController.instance.Respawn();
        }
    }
}
