using UnityEngine;

public class PlayerCombat : CombatController
{
    [Header("References")]
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerController playerController;

    [Header("Attack Settings")]
    public Transform[] attackSides;
    public float attackRange;
    public LayerMask enemies;
    public GameObject swordSlashPrefab;

    private Vector3 attackLocation;
    private bool isMeleeAttacking;
    private bool isShootAttacking;
    private GameObject currentSwordSlash;

    [Header("Audio")]
    [SerializeField] private AudioClip swordSoundClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (UIHandler.instance.isPaused)
        {
            return;
        }

        if (!playerController.active || playerMovement.isSliding) return;

        UpdateAttackLocation();
        // ShootingAttack();
        MeleeAttack();
    }

    private void UpdateAttackLocation()
    {
        attackLocation = playerController.direction < 0 ? attackSides[0].position : attackSides[1].position;
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

    private void MeleeAttack()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isMeleeAttacking)
        {
            animator.SetTrigger("Melee");
            isMeleeAttacking = true;
            SoundFXManager.instance.PlaySoundFXClip(swordSoundClip, transform, 1, false);
        }
    }

    public void Slash()
    {
        float attackDirection = playerController.direction < 0 ? -1 : 1;
        currentSwordSlash = Instantiate(swordSlashPrefab, attackLocation, Quaternion.identity);

        Vector3 scale = currentSwordSlash.transform.localScale;
        scale.x *= attackDirection;
        currentSwordSlash.transform.localScale = scale;

        Quaternion rotation = currentSwordSlash.transform.rotation;
        rotation.y = transform.rotation.y;
        currentSwordSlash.transform.rotation = rotation;
        Destroy(currentSwordSlash, 1);
    }

    public void OnMeleeAttackAnimationComplete()
    {
        isMeleeAttacking = false;

        if (currentSwordSlash != null)
        {
            Destroy(currentSwordSlash);
        }
    }

    public void ResetAttack()
    {
        isMeleeAttacking = false;
    }

    public void CheckIfEnemyIsHit()
    {
        // Detect enemies within the attack range
        Collider[] damage = Physics.OverlapSphere(attackLocation, attackRange, enemies);

        foreach (var collider in damage)
        {
            var enemy = collider.GetComponent<HealthController>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
            else
            {
                var lever = collider.GetComponent<Lever>();
                if (lever != null)
                {
                    lever.Switch();
                }
                 
                var boss = collider.GetComponent<BossStateController>();
                if (boss != null)
                {
                    boss.TakeDamage();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (attackSides != null && attackSides.Length > 1)
        {
            Gizmos.DrawWireSphere(attackSides[0].position, attackRange);
            Gizmos.DrawWireSphere(attackSides[1].position, attackRange);
        }
    }
}
