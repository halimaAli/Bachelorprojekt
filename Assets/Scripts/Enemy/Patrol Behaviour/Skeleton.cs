using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField] private float idleTime = 2f;
    [SerializeField] protected float attackRange = 1.5f;

    private bool isIdle = false;
    private float idleTimer = 0f;

    public override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!attacking)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (!isIdle)
        {
            Move();
        }
        else
        {
            // Skeleton is Idle for movementDirection few seconds
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTime)
            {
                isIdle = false;
                idleTimer = 0f;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Wall"))
        {
            Idle();
            isIdle = true;
        }
    }
}
