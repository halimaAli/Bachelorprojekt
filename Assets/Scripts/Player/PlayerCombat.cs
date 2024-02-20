using System;
using UnityEngine;

public class PlayerCombat : CombatController
{

    private Animator animator;
    public float attackTime;
    public float startTimeAttack;

    public Transform attackLocation;
    public float attackRange;
    public LayerMask enemies;
    private bool isMeleeAttacking;
    private bool isShootAttacking;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
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
        // Reset the flag to indicate that the melee attack is no longer in progress
        isMeleeAttacking = false;
        animator.SetBool("meleeAttack", false);
    }
 

    public void CheckIfEnemyIsHit()
    {
        // Detect enemies within the attack range
        Collider[] damage = Physics.OverlapSphere(attackLocation.position, attackRange, enemies);

        // Damage and destroy each enemy
        for (int i = 0; i < damage.Length; i++)
        {
            Destroy(damage[i].gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
