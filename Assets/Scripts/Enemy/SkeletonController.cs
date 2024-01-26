using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    private float distance;
    [SerializeField]
    private Transform player;
    private Enemy_Movement enemy_Movement;
    private float attackDistance;
    private Animator animator;
    private float treshold;
    private bool isAttacking;

    // Start is called before the first frame update
    void Awake()
    {
        enemy_Movement = GetComponent<Enemy_Movement>();
        animator = GetComponent<Animator>();
        attackDistance = 2.0f;
        treshold = 0.4f;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance <= attackDistance && PlayerController.instance.active)
        {
            AttackPlayer();
        }
        else if (!isAttacking)
        {
            animator.SetBool("isAttacking", false);
            enemy_Movement.MoveLeftandRight();
           
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
        if (distance <= attackDistance + treshold)
        {
            PlayerController.instance.Die(false);
        }
        isAttacking = false;
    }
}
