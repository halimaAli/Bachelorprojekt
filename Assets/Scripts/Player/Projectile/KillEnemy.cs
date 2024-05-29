using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<HealthController>();
        var boss1 = other.GetComponent<BossStateController>();

        if (enemy != null)
        {
            enemy.TakeDamage();
            Destroy(gameObject);
        }
        else if (boss1 != null)
        {
            boss1.TakeDamage();
            Destroy(gameObject);
        }
        else if (other.gameObject.tag.Equals("Enemy")) // An enemy projectile was hit
        {
            Destroy(other.gameObject);
            Destroy(gameObject); 
        }         
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
 