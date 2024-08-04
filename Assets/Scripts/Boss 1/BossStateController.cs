using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossStateController : MonoBehaviour
{
    public static BossStateController instance;

    [Header("Components")]
    public GameObject player;
    private Animator animator;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;

    private float localScaleX;
    public int direction;
    public new bool enabled;
    [SerializeField] internal int health;

    [Header("Summon Phase Parameters")]
    [SerializeField] private Transform platform;
    [SerializeField] private float jumpForce;

    [Header("Attack Parameters")]
    [SerializeField] private float comboDistance = 10f;
    [SerializeField] private float spinDistance = 5f;
   // [SerializeField] private float walkDistance = 20f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float movementSpeed = 3f;
    private float nextAttackTime;
    private int maxHealth;
    [SerializeField] private Image healthBar;
    private BossLevelManager.Phase currentPhase;

    void Awake()
    {
        if (instance == null) instance = this;
       
        enabled = true;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        localScaleX = transform.localScale.x;
        nextAttackTime = Time.time + attackCooldown;

        maxHealth = health;
    }

    private void Start()
    {
        //StartCoroutine(Intro());
    }

    void Update()
    {
        if (enabled)
        {
            FacePlayer();
        }

        healthBar.fillAmount = Mathf.Clamp((float)health / maxHealth, 0, 1);

        CheckHealth();

        if (Camera.main.orthographic && currentPhase != BossLevelManager.Phase.SummonPhase)
        {
            if (Time.time >= nextAttackTime)
            {
                DecideAction(BossLevelManager.instance.getcurrentPhase() == BossLevelManager.Phase.AttackPhase2);
            }
            else
            {
                Idle();
            }
        }
    }

    #region Attack Phase
    void DecideAction(bool lastPhase)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= spinDistance)
        {
            animator.SetBool("isWalking", false);
            SpinAttack();
        }
        else if (distanceToPlayer <= comboDistance)
        {
            animator.SetBool("isWalking", false);
            ComboAttack();
        }
        else if (lastPhase)
        {
            JumpAttack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }
    #endregion

    #region Summon Phase
    private void SummonAnimation()
    {
        enabled = false;
        FaceDirection(platform);
        animator.SetTrigger("Jump");
    }

    public void InitateSummonPhase()
    {
        BossLevelManager.instance.setPhase(2);
    } 
    #endregion

    #region Movement
    void MoveTowardsPlayer()
    {
        // Move towards the player
        animator.SetBool("isWalking", true);
        transform.Translate(direction * Vector3.right * movementSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        // Calculate position of platform
        Vector3 direction = (platform.position - transform.position).normalized;
        rb.AddForce(new Vector3(direction.x * jumpForce, jumpForce, 0), ForceMode.Impulse);
    }

    public void Fall()
    {
        rb.velocity = Vector3.zero;
    }
    #endregion

    #region Start Attacks
    public void SpinAttack()
    {
        animator.SetTrigger("SpinAttack");
        nextAttackTime = Time.time + attackCooldown;
    }

    public void ComboAttack()
    {
        animator.SetTrigger("ComboAttack");
        nextAttackTime = Time.time + attackCooldown;
    }

    public void JumpAttack()
    {
         animator.SetTrigger("JumpAttack");
    }
    #endregion

    #region Health
    private void CheckHealth()
    {
        if (health == 10 && currentPhase != BossLevelManager.Phase.SummonPhase)
        {
            currentPhase = BossLevelManager.Phase.SummonPhase;
            SummonAnimation();
        }
    }

    public void TakeDamage()
    {
        health--;

        print("Health: " + health);

        if (health == 0)
        {
            animator.SetTrigger("Dead");
        }
        else
        {
            StartCoroutine(GetHit());
        }
    }

    private IEnumerator GetHit()
    {
        Color originalColor =  spriteRenderer.color;
        spriteRenderer.material.color = Color.white;
        yield return new WaitForSeconds(1f);
        spriteRenderer.material.color = originalColor;
    }

    #endregion

    #region Util
    public void Idle()
    {
        enabled = true;  
    }

    public void FacePlayer()
    {
        FaceDirection(player.transform);
    }

    private void FaceDirection(Transform obj)
    {
        float distance = obj.position.x - transform.position.x;

        if (distance > 0) //Player is on the  LEFT  side of the boss
        {
            direction = 1;
            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }
        else //Player is on the  RIGHT  side of the boss
        {
            direction = -1;
            transform.localScale = new Vector3(-1 * localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }
    #endregion
}
