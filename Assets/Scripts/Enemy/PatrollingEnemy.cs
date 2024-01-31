using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrollingEnemy : EnemyMovements
{
    [SerializeField] private Transform player;

    [SerializeField] private float patrolRange;

    private float distance;
    private float attackDistance;
    private float threshold;
    private bool isAttacking;
    private float startPosX;

    void Awake()
    {
        attackDistance = 2.0f;
        threshold = 0.4f;
        isAttacking = false;
        startPosX = Mathf.Abs(transform.position.x);
    }

    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);

        //if Enemy is close to Player & Player is alive -> Attack
        if (distance <= attackDistance && PlayerController.instance.active)
        {
            AttackPlayer();
        }
        else if (!isAttacking)
        {
            animator.SetBool("isAttacking", false);
            Patrol();
        }
    }


    public void Patrol()
    {
       if (idle){return;} //do nothing if enemy is idle

        float pointA = startPosX + patrolRange;

        float pointB = startPosX - patrolRange;

        MoveLeftandRight();

        if (Mathf.Abs(transform.position.x) >= pointA || Mathf.Abs(transform.position.x) <= pointB)
        {   
            StartCoroutine(Idle(3.0f)); //idle for 3 sec
        }
    }


    private void AttackPlayer()
    {
        animator.SetBool("isAttacking", true);
        isAttacking = true;
    }

    public void AttackAnimationEnded()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance <= attackDistance + threshold)
        {
            //Kill Player if Attack-Animation ended
            PlayerController.instance.Die(false);
        }
        isAttacking = false;
    }
}
