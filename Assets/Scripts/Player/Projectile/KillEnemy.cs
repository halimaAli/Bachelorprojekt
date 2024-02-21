using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyController>();



        if (enemy != null)
        {
            enemy.TakeDamage();
            Destroy(gameObject);
        }

       /* if (other.gameObject.tag.Equals("Enemy"))
       {
            Destroy(other.gameObject);
            Destroy(gameObject);
       }*/
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage();
        }
    }*/
}
 