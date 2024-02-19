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

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("shootAttack", true);
            ShootProjectile();
        }

        MeleeAttack();
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //animator.SetBool("meleeAttack", true);
            MeleeAttack();
        }*/
    }

    public void MeleeAttack()
    {
        // Check if the player presses the mouse button
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMeleeAttacking)
        {
            // Set the meleeAttack parameter of the animator to true
            animator.SetBool("meleeAttack", true);
            
        }
    }

    public void OnMeleeAttackAnimationComplete()
    {
        // Reset the flag to indicate that the melee attack is no longer in progress
        isMeleeAttacking = false;

        // Set the meleeAttack parameter of the animator to false
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
