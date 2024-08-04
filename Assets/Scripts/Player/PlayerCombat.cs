using System;
using UnityEngine;

public class PlayerCombat : CombatController
{

    private Animator animator;
    private PlayerMovement playerMovement;

    public Transform[] attackSides;
    private Vector3 attackLocation;
    public float attackRange;
    public LayerMask enemies;
    private bool isMeleeAttacking;
    private bool isShootAttacking;
    public GameObject swordSlashPrefab;
    private GameObject currentSwordSlash;

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Melee");
            isMeleeAttacking = true;

            currentSwordSlash = Instantiate(swordSlashPrefab, attackLocation, Quaternion.identity);

            Vector3 scale = currentSwordSlash.transform.localScale;
            scale.x *= PlayerController.instance.direction < 0 ? -1 : 1;
            currentSwordSlash.transform.localScale = scale;
        }
    }

    public void OnMeleeAttackAnimationComplete()
    {
        isMeleeAttacking = false;

        if (currentSwordSlash != null)
        {
            Destroy(currentSwordSlash);
        }
    }

    public void CheckIfEnemyIsHit()
    {
        // Detect enemies within the attack range
        //TODO: add boss
        Collider[] damage = Physics.OverlapSphere(attackLocation, attackRange, enemies);

        for (int i = 0; i < damage.Length; i++)
        {
            var enemy = damage[i].GetComponent<HealthController>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            } else
            {
                var lever = damage[i].GetComponent<Lever>();
                lever.Switch();
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
