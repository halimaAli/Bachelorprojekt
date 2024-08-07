
using UnityEngine;

public class PlayerCombat : CombatController
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerController playerController;

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
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.active || playerMovement.isSliding) return;

        if (playerController.direction < 0)
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
            animator.SetTrigger("Melee");
            isMeleeAttacking = true;
            playerMovement.enabled = false; //Disable movement during melee attack
        }
    }


    public void Slash()
    {
        float attackDirection = PlayerController.instance.direction < 0 ? -1 : 1;
        currentSwordSlash = Instantiate(swordSlashPrefab, attackLocation, Quaternion.identity);

        Vector3 scale = currentSwordSlash.transform.localScale;
        scale.x *= attackDirection;
        currentSwordSlash.transform.localScale = scale;

        Quaternion rotation = currentSwordSlash.transform.rotation;
        rotation.y = transform.rotation.y;
        currentSwordSlash.transform.rotation = rotation;
    }

    public void OnMeleeAttackAnimationComplete()
    {
        isMeleeAttacking = false;
        playerMovement.enabled = true; //Reenable movement after melee attack

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
