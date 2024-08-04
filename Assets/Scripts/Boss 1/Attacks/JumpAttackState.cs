using UnityEngine;

public class JumpAttackState : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private GroundChecker groundChecker;

    [Header("Attack Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;

    [Header("References")]
    public GameObject player;
    [SerializeField] private GameObject hammer;
    [SerializeField] private Transform hammerPos;
    
    private bool isDashing;
    private Vector3 direction;
    public bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundChecker = GetComponent<GroundChecker>();
    }

    private void Update()
    {
        isGrounded = groundChecker.CheckGround();

        if (isDashing)
        {
            HandleDash();
        }
    }

    private void HandleDash()
    {
        if (!isGrounded)
        {
            animator.speed = 0;
            transform.Translate(direction * dashForce * Time.deltaTime);
        }
        else
        {
            animator.speed = 1;
            isDashing = false;
        }
    }

    public void OnStartJumpAttack()
    {
        BossStateController.instance.enabled = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void OnStayInAir()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    public void ThrowHammer()
    {
        Instantiate(hammer, hammerPos.position, hammerPos.rotation);
        direction = player.transform.position - transform.position;

        //Boss doesn't land exactly on Player but movementDirection bit to the left
        direction.x -= (4 * BossStateController.instance.direction); 
    }

    public void OnDashDown()
    {
        rb.useGravity = true;
        isDashing = true;
    }

    public void OnLanded()
    {
        isDashing = false;
        rb.velocity = Vector3.zero;
    }
}
