using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private DamageType damageType;

    private Animator _animator;

    private enum DamageType
    {
        InstantDeath,
        DestructibleProjectile,
        Enemy,
        AnimatedProjectile
    }

    private void Start()
    {
        if (damageType == DamageType.AnimatedProjectile)
        {
            _animator = GetComponent<Animator>();
        }
    }

    private void HandlePlayerCollision(PlayerController player)
    {
        if (damageType == DamageType.InstantDeath)
        {
            player.Die();
        }
        else
        {
            player.TakeDamage();
        }
    }

    private void OnCollisionEnter(Collision collision) // Player collides with enemy
    {
        var player = collision.collider.GetComponent<PlayerController>();

        if (player != null && damageType == DamageType.Enemy)
        {
            HandlePlayerCollision(player);
        }
    }

    private void OnTriggerEnter(Collider other) // Player collides with projectile
    {
        var player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            HandlePlayerCollision(player);

            // For projectiles, destroy immediately if they are destructible or animated
            if (damageType == DamageType.DestructibleProjectile || damageType == DamageType.AnimatedProjectile)
            {
                PlayAnimationAndDestroy();
            }
        }
        else if (damageType == DamageType.AnimatedProjectile && other.CompareTag("ground"))
        {
            PlayAnimationAndDestroy();
        }
    }

    private void PlayAnimationAndDestroy()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Destroyed");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
