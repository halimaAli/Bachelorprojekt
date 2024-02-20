using UnityEngine;

public class EnemyDetection : EnemyMovements
{
    [SerializeField] protected Transform player;


    private Vector3 startPosition;
    private float detectionRadius = 5f;
    private bool canChase;
    protected float distance;


    void Awake()
    {
        startPosition = transform.position;
        canChase = false;
    }

    void Update()
    {
        if (!PlayerController.instance.active)     // Player is dead, do not continue chasing
        {
            ReturnToStartPosition();
            canChase = false;
            return;
        }

        distance = Vector3.Distance(player.position, transform.position);

        if (distance < detectionRadius) // Player is within detection radius, start chasing
        {
            canChase = true;
        }

        if (canChase)
        {
            ChasePlayer();
        }
    }

    protected void ChasePlayer()
    {
        MoveTo(player.position, true);
    }

    protected void ReturnToStartPosition()
    {
        MoveTo(startPosition, false);
    }
}
