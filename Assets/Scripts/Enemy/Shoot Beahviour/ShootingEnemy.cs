using UnityEngine;

public class ShootingEnemy : Enemy
{
    [Header("Shooting Settings")]
    [SerializeField] private EnemyPosition enemyPosition;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float detectionRadius = 5f;

    private float shootTimer;

    private enum EnemyPosition
    {
        Air,
        Ground
    }

    public override void Start()
    {
        base.Start();
        shootTimer = shootCooldown;
    }

    protected override void Update()
    {
        base.Update();

        if (distance <= detectionRadius)
        {
            detectedPlayer = true;
        }
        else
        {
            detectedPlayer = false;
            attacking = false;
        }

        if (detectedPlayer)
        {
            FaceDirection(player.position);
            if (!attacking && shootTimer <= 0f)
            {
                ShootAtPlayer();
            }
        }
        else
        {
            Idle();
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    private void ShootAtPlayer()
    {
        _animator.SetTrigger("Shoot");
        attacking = true;
        shootTimer = shootCooldown;
    }


    public void FireProjectile()
    {
        Vector3 directionToPlayer;
        if (enemyPosition == EnemyPosition.Air)
        {
            directionToPlayer = ((player.position + new Vector3(0, 1, 0)) - transform.position).normalized;
        } else
        {
            Vector3 calculatedDirection = (player.position - transform.position).normalized;
            directionToPlayer = new Vector3(calculatedDirection.x, 0, 0);
        }

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = directionToPlayer * speed;

        Destroy(projectile, 10); // automatically destroy the projectile when it didn't hit anything
        attacking = false;
    }

    protected override void Idle()
    {
        base.Idle();
        attacking = false;
    }
}
