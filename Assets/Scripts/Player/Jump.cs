using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float doubleJumpForce = 8f;
    public float jumpTime = 0.35f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool canDoubleJump;
    private float jumpTimeCounter;
    private bool isJumping;
    private bool jumpInputReleased = true;

    private GroundChecker groundChecker;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = GetComponent<GroundChecker>();
        isGrounded = true;
    }

    void Update()
    {
        CheckIfGrounded();
        HandleJumpInput();
    }
   
    private void CheckIfGrounded()
    {
        bool grounded = groundChecker.CheckGround();
        if (!isGrounded && grounded)
        {
            isGrounded = true;
            
        }
        else if (isGrounded && !grounded)
        {
            isGrounded = false;
        }
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || canDoubleJump))
        {
            if (isGrounded)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (canDoubleJump)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                canDoubleJump = false;
            }
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }
}

