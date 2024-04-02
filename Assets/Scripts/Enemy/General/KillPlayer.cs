using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) // player collides with enemy
    {
        var player = collision.collider.GetComponent<PlayerController>();

        if (player != null )
        {
            if (tag.Equals("Pikes"))
            {
                if (Camera.main.orthographic)
                {
                    player.Die(false);
                }
                return;
            }


            player.TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other) // player collides with projectile (e.g. arrow)
    {
        var player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            Destroy(gameObject);
            player.TakeDamage();
        }
    }


}
