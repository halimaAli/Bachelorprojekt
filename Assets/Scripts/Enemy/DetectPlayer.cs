using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer spriteRenderer;
   
    private float chaseSpeed = 3f;
    private float returnSpeed = 1.5f;
    private Animator ani;
    public Vector3 startPosition;
    public float detectionRadius = 5f;
    public float maxChaseDistance = 10f;


    void Awake()
    {
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    void Update()
    {

        if (!PlayerController.instance.active)     // Player is dead, do not continue chasing
        {
            ReturnToStartPosition();
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);


        if (distance < detectionRadius) // Player is within detection radius, start chasing
        {
            ChasePlayer();
        }
        else if (distance > maxChaseDistance) // Player is too far, return to start position
        {
            ReturnToStartPosition();
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        ani.SetBool("isAttacking", true);
        spriteRenderer.flipX = true;
    }

    private void ReturnToStartPosition()
    {
        transform.position = Vector3.Lerp(transform.position, startPosition, returnSpeed * Time.deltaTime);
        ani.SetBool("isAttacking", false);
        spriteRenderer.flipX = false;
    }
}
