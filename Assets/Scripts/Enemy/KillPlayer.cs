using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private Type type;
    private enum Type
    {
        Projectile,
        Boss_Hammer,
        Enemy,
        Lava
    }

    private void OnCollisionEnter(Collision collision) // player collides with enemy
    {
        var player = collision.collider.GetComponent<PlayerController>();

        if (player != null )
        {
            player.TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other) // player collides with projectile (e.g. arrow)
    {
        var player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (type == Type.Projectile)
            {
                Destroy(gameObject);
            }
            else if (other.tag.Equals("Boss") && type == Type.Boss_Hammer)
            {
                Destroy(gameObject);
            }
            else if (type == Type.Lava)
            {
                player.Die(false);
                return;
            } 

            player.TakeDamage();
        }

       
    }


}
