using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag.Equals("Enemy"))
       {
            Destroy(other.gameObject);
            Destroy(gameObject);
       }
    }
}
 