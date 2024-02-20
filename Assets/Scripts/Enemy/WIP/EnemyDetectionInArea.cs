using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetectionInArea : EnemyDetection
{
    private bool atStartPos = true;
    private bool isAttackAnimationComplete = true;
    public bool playerDetected;
    private float attackDistance = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (!playerDetected && isAttackAnimationComplete && !atStartPos)
        { 
            ReturnToStartPosition();
            atStartPos = true;
            return;
        }
        else
        {
            distance = Vector3.Distance(player.position, transform.position);

            if (distance <= attackDistance)
            {
                atStartPos = false;
                AttackPlayer();
            }
            else
            {
                if (isAttackAnimationComplete)
                {
                    atStartPos = false;
                    ChasePlayer();
                }

            }
        }
    }

    private void AttackPlayer()
    {
        animator.SetBool("isAttacking", true);
        isAttackAnimationComplete = false;
    }

    public void AttackAnimationEnded()
    {
        // Check if the player is still close after the attack animation
        distance = Vector3.Distance(player.position, transform.position);
        if (distance <= attackDistance + .4f)
        {
            PlayerController.instance.Die(false);
            playerDetected = false;
        }

        animator.SetBool("isStanding", true);
        isAttackAnimationComplete = true;
    }

}
