using System;
using UnityEngine;

public class PlayerCombat : CombatController
{

    private Animator animator;
    private PlayerMovement playerMovement;
    public float attackTime;
    public float startTimeAttack;

    public Transform[] attackSides;
    private Vector3 attackLocation;
    public float attackRange;
    public LayerMask enemies;
    private bool isMeleeAttacking;
    private bool isShootAttacking;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!PlayerController.instance.active || playerMovement.isSliding) return;

        if (PlayerController.instance.direction < 0)
        {
            attackLocation = attackSides[0].position;
        } else
        {
            attackLocation = attackSides[1].position;
        }

        ShootingAttack();
        MeleeAttack();
    }

    private void ShootingAttack()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isShootAttacking)
        {
            animator.SetBool("shootAttack", true);
            ShootProjectile();
            isShootAttacking = true;
        }
    }

    public void OnShootingAttackAnimationComplete()
    {
        animator.SetBool("shootAttack", false);
        isShootAttacking = false;  
    }

    public void MeleeAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMeleeAttacking)
        {
            animator.SetBool("meleeAttack", true);
            isMeleeAttacking = true;
        }
    }

    public void OnMeleeAttackAnimationComplete()
    {
        isMeleeAttacking = false;
        animator.SetBool("meleeAttack", false);
    }

    public void CheckIfEnemyIsHit()
    {
        // Detect enemies within the attack range
        Collider[] damage = Physics.OverlapSphere(attackLocation, attackRange, enemies);

        for (int i = 0; i < damage.Length; i++)
        {
            var enemy = damage[i].GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackSides[0].position, attackRange);
        Gizmos.DrawWireSphere(attackSides[1].position, attackRange);
    }
}
