using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponent<PlayerController>();

        if (player != null )
        {
            player.Die(false);
        }
    }
}
