using UnityEngine;

public class EnemyDetection : EnemyMovements
{
    [SerializeField] private Transform player;


    private SpriteRenderer spriteRenderer;
    private float moveSpeed = 3f;
    private Animator ani;
    private Vector3 startPosition;
    private float detectionRadius = 5f;
    private bool canChase;


    void Awake()
    {
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < detectionRadius) // Player is within detection radius, start chasing
        {
            canChase = true;
        }

        if (canChase)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        MoveTo(player.position, true);
    }

    private void ReturnToStartPosition()
    {
        MoveTo(startPosition, false);
    }
}
