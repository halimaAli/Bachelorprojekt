using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStateController : MonoBehaviour
{
    public static BossStateController instance;

    [Header("Demo")]
    public Demo demo;

    [Header("Components")]
    public GameObject player;
    private Animator animator;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider boxcollider;
    private GroundChecker groundChecker;
    private JumpAttackState jumpAttackState;
    private BossDialogue bossDialogue;

    private float localScaleX;
    public int direction;
    public bool isGrounded;

    [Header("Boss Stats")]
    public bool attacking;
    public Attack nextAttack;
    public int health;
    private int maxHealth;
    public bool introPlaying;

    [Header("Attack Parameters")]
    [SerializeField] private float longRange = 13f;
    [SerializeField] private float shortRange = 8f;
    [SerializeField] private float movementSpeed = 3f;
    private float timeBetweenAttacks = 4.0f;
    private float attackCooldown = 0.0f;

    public float horizontalForce = -15f;
    public float verticalForce = 0f;

    [Header("Summon Phase Parameters")]
    [SerializeField] private Transform jumpPosition;
    [SerializeField] private Transform landingPosition;
    public bool isSummoning = false;
    private bool isJumping;
    private bool hasSummonedEndPhase = false;
    [SerializeField] private MovingWall wall;

    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text hp;

    [Header("AudioClips")]
    [SerializeField] private AudioClip landingSoundClip;
    [SerializeField] private AudioClip landing2SoundClip;
    [SerializeField] private AudioClip roarSoundClip;
    [SerializeField] private AudioClip defeatSoundClip;
    [SerializeField] private AudioClip jumpSoundClip;
    [SerializeField] private AudioClip summonSoundClip;

    private bool lookAtPlayer;

    public bool reached;
    internal bool inactive;

    private BossLevelManager.Phase currentPhase;
    

    public enum Attack
    {
        Empty,
        SpinAttack,
        ComboAttack,
        JumpAttack
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }

        lookAtPlayer = true;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxcollider = GetComponent<BoxCollider>();
        groundChecker = GetComponent<GroundChecker>();
        jumpAttackState = GetComponent<JumpAttackState>();
        bossDialogue = GetComponent<BossDialogue>();
        demo = GetComponent<Demo>();

        localScaleX = transform.localScale.x;
        maxHealth = health;
    }

    private void Start()
    {
        StartCoroutine(Intro());
    }

    void Update()
    {
        if (inactive)
        {
            return;
        }

        currentPhase = BossLevelManager.instance.getcurrentPhase();

        if (lookAtPlayer)
        {
            FacePlayer();
        }

        UpdateHealthBar();
        isGrounded = groundChecker.CheckGround();

        if (introPlaying) return;

        CheckHealth();

        if (isSummoning)
        {
            InitiateSummonPhase();
            return; 
        }

        if (demo != null && demo.isDemo)
        {
            return;
        }
        
        if (currentPhase != BossLevelManager.Phase.SummonPhase)
        {
            if (!attacking)
            {
                int attackOptions = (currentPhase == BossLevelManager.Phase.AttackPhase1) ? 2 : 3;

                if (nextAttack == Attack.Empty)
                {
                    DecideNextAttack(attackOptions);
                }
                PhaseLogic();
            }
            else
            {
                HandleAttackCooldown();
            }
        }
    }

    #region Attack Logic
    private void DecideNextAttack(int numOfAttacks)
    {
        int rand = Random.Range(1, numOfAttacks + 1);
        switch (rand)
        {
            case 1:
                nextAttack = Attack.SpinAttack;
                break;
            case 2:
                nextAttack = Attack.ComboAttack;
                break;
            case 3:
                nextAttack = Attack.JumpAttack;
                break;
        }
    }

    private void PhaseLogic()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (nextAttack == Attack.SpinAttack)
        {
            if (distanceToPlayer <= shortRange)
            {
                StopMoving();
                SpinAttack();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        else if (nextAttack == Attack.ComboAttack)
        {
            if (distanceToPlayer >= longRange || reached)
            {
                StopMoving();
                ComboAttack();
            }
            else
            {
                MoveAwayFromPlayer();
            }
        }
        else if (nextAttack == Attack.JumpAttack)
        {
            if (distanceToPlayer >= longRange || reached) 
            {
                if (PlayerController.instance.IsGrounded)
                {
                    StopMoving();
                    JumpAttack();
                } else
                {
                    MoveAwayFromPlayer();
                }
            }
            else
            {
                MoveAwayFromPlayer();
            }
        }
    }

    public void SpinAttack()
    {
        attacking = true;
        animator.SetTrigger("SpinAttack");
        attackCooldown = timeBetweenAttacks;
    }

    public void ComboAttack()
    {
        attacking = true;
        animator.SetTrigger("ComboAttack");
        attackCooldown = timeBetweenAttacks;
        reached = false;
    }

    public void JumpAttack()
    {
        attacking = true;
        animator.SetTrigger("JumpAttack");
        attackCooldown = timeBetweenAttacks;
        reached = false;
    }

    private void HandleAttackCooldown()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attacking = false;
            nextAttack = Attack.Empty;
        }
    }
    #endregion

    #region Movement
    private void MoveTowardsPlayer()
    {
        animator.SetBool("isWalking", true);
        transform.Translate(direction * Vector3.right * movementSpeed * Time.deltaTime);
    }

    private void MoveAwayFromPlayer()
    {
        FaceAwayFromPlayer();
        MoveTowardsPlayer(); // Use same movement logic but reverse the direction
    }

    private void StopMoving()
    {
        animator.SetBool("isWalking", false);
    }

    public void Idle()
    {
        lookAtPlayer = true;
    }
    #endregion

    #region Health Management
    private void CheckHealth()
    {
        if (health == 20 && currentPhase != BossLevelManager.Phase.SummonPhase && !isSummoning)
        {
            isSummoning = true;
        }
    }

    public void TakeDamage()
    {
        health--;
        UpdateHealthBar();

        if (health == 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(GetHit());
        }
    }

    public void DemoBossDie()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        rb.useGravity = false;
        boxcollider.enabled = false;
        inactive = true;
        animator.SetTrigger("Dead");
        SoundFXManager.instance.PlaySoundFXClip(defeatSoundClip, transform, 1, false);
        yield return new WaitForSeconds(5f);
        BossLevelManager.instance.Result(true);
    }

    private IEnumerator GetHit()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.5f);
        spriteRenderer.material.color = originalColor;
    }

    private void UpdateHealthBar()
    {
        hp.text = health.ToString();
        healthBar.fillAmount = Mathf.Clamp((float)health / maxHealth, 0, 1);
    }
    #endregion

    #region Summon Sequence
    public void InitiateSummonPhase()
    {
        FaceDirection(jumpPosition.position);
        if (!HasReachedJumpPosition() && !isJumping)
        {
            MoveTowardsPlayer(); // Moving towards jump position
        }
        else if (!isJumping)
        {
            StopMoving();
            animator.SetTrigger("Jump");
            isJumping = true;
        }
    }


    private bool HasReachedJumpPosition()
    {
        float distanceToJumpPosition = Vector3.Distance(transform.position, jumpPosition.position);
        return distanceToJumpPosition <= 0.4f;
    }

    public void PerformJumpToPlatform()
    {
        float horizontalForce = 30f;
        float verticalForce = 40f;

        rb.AddForce(new Vector3(-horizontalForce, verticalForce, 0), ForceMode.Impulse);
        SoundFXManager.instance.PlaySoundFXClip(jumpSoundClip, transform, 1, false);
    }

    public void Fall()
    {
        SoundFXManager.instance.PlaySoundFXClip(landing2SoundClip, transform, 1, false);
        rb.velocity = Vector3.zero;
        StartCoroutine(SummonAnimation());
    }

    private IEnumerator SummonAnimation()
    {
        yield return new WaitUntil(() => isGrounded);
        yield return new WaitForSeconds(.5f);

        animator.SetTrigger("Taunt");
        SoundFXManager.instance.PlaySoundFXClip(roarSoundClip, transform, 1, false);

        yield return new WaitForSeconds(.5f);
        wall.Move();
        yield return new WaitForSeconds(2f);
        BossLevelManager.instance.setPhase(2);
    }

    public void EndSummonPhase()
    {
        hasSummonedEndPhase = true;
        StartCoroutine(SummonPhaseEnd());
    }

    private IEnumerator SummonPhaseEnd()
    {
        jumpAttackState.SetTarget(landingPosition.gameObject);
        JumpAttack();

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => isGrounded);
        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Taunt");
        SoundFXManager.instance.PlaySoundFXClip(roarSoundClip, transform, 1, false);
        isSummoning = false;
        health = 15;
        jumpAttackState.SetTarget(player);
        wall.MoveBack();
        attacking = false;
        nextAttack = Attack.Empty;
    }
    #endregion

    #region Intro Sequence
    private IEnumerator Intro()
    {
        introPlaying = true;
        rb.useGravity = true;
        yield return new WaitForSeconds(.5f);
        
        SmashIntoGround();

        yield return new WaitUntil(() => isGrounded);
        yield return new WaitForSeconds(.5f);

        animator.SetTrigger("Taunt");
        SoundFXManager.instance.PlaySoundFXClip(roarSoundClip, transform, 1, false);
    }

    private void SmashIntoGround()
    {
        PlayerController.instance.active = false;
        animator.SetTrigger("Intro");
        float smashForce = -500f;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector3(0, smashForce, 0), ForceMode.Impulse);
        SoundFXManager.instance.PlaySoundFXClip(landingSoundClip, transform, 1, false);
    }

    public void IntroEnd()
    {
        PlayerController.instance.active = true;
        introPlaying = false;
    }
    #endregion

    #region Utility
    public void FacePlayer()
    {
        FaceDirection(player.transform.position);
    }

    public void FaceAwayFromPlayer()
    {
        FaceDirection(player.transform.position * -1);
    }

    private void FaceDirection(Vector3 dir)
    {
        float distance = dir.x - transform.position.x;

        if (distance > 0)
        {
            direction = 1;
            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            direction = -1;
            transform.localScale = new Vector3(-1 * localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }
    
    public void Won()
    {
        animator.SetBool("isWalking", false);
        inactive = true;
        bossDialogue.isPlayerClose = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            reached = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            reached = false;
        }
    }

    internal void Respawn() //Sometimes Boss just dashes throught the level borders
    {
        rb.velocity = Vector3.zero;
        attacking = false;
        nextAttack = Attack.Empty;
        transform.position = new Vector3(-20, 0, 0f);
    }

    #endregion
}
