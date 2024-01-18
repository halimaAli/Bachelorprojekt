using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponent<PlayerController>();

        if (player != null )
        {
            if (tag.Equals("Pikes") && !Camera.main.orthographic) return;

            player.Die(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            Destroy(gameObject);
            player.Die(false);
        }
    }


}
