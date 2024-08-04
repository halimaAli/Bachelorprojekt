using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider boxCollider;

    [Header("Movement")]
    public float horizontalInput;
    private float verticalInput;
    public float walkingSpeed;

    [Header("Jumping")]
    public float jumpForce = 50.0f;
    public float fallMultiplier = 1.5f;
    public float lowJumpMultiplier = 2f;
    internal bool isJumping;

    [Header("GroundCheck")]
    private GroundChecker groundChecker;
    public bool isGrounded;

    [Header("Crouching")]
    private bool hasDoubleJumped;
    private float lastJumpTime;
    internal bool isSliding;

    void Start()
     {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        groundChecker = GetComponent<GroundChecker>();
        isGrounded = true;
    }

    void Update()
     {
        horizontalInput = Input.GetAxis("Horizontal"); // A and D Input
        verticalInput = Input.GetAxis("Vertical"); //// W and S Input
        if (!PlayerController.instance.active) return;
        HandleWalking();
        CheckIfGrounded();
        HandleJumping();
        HandleCrouching();
     }

    #region Walk
    private void HandleWalking()
     {
        animator.SetBool("isWalking", Camera.main.orthographic? horizontalInput != 0 : horizontalInput != 0 || verticalInput != 0);
        
        if (Camera.main.orthographic)
        {
            if (horizontalInput > 0)
            {
                spriteRenderer.flipX = false;
                PlayerController.instance.direction = 1;
            }
            else if (horizontalInput < 0)
            {
                spriteRenderer.flipX = true;
                PlayerController.instance.direction = -1;
            }
            transform.Translate(Vector3.right * walkingSpeed * horizontalInput * Time.deltaTime);
        }
        else
        {
            int invert = 1;
            PlayerController.instance.direction = 1;
            if (CameraManager.instance.isBackwards3D)
            {
                invert = -1;
                PlayerController.instance.direction = -1;
            }

            if (horizontalInput > 0)
            {
                spriteRenderer.flipX = invert < 0;
            }
            else if (horizontalInput < 0)
            {
                spriteRenderer.flipX = invert > 0;
            }

            transform.Translate(Vector3.right * walkingSpeed * horizontalInput * invert * Time.deltaTime);
            transform.Translate(Vector3.forward * walkingSpeed * verticalInput * invert * Time.deltaTime);
        }
     }
    #endregion

    #region GroundCheck

    private void CheckIfGrounded()
    {
        bool grounded = groundChecker.CheckGround();
        if (!isGrounded && grounded)
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("doubleJump", false);
        }
        else if (isGrounded && !grounded)
        {
            isGrounded = false;
            transform.SetParent(null);
        }
    }
    #endregion

    #region Jump

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float timeSinceLastJump = Time.time - lastJumpTime;
            if (isGrounded) //start normal jump
            {
                animator.SetBool("isJumping", true);
                rb.velocity = Vector3.up * jumpForce;
                hasDoubleJumped = false;

                animator.SetFloat("yVelocity", rb.velocity.y);

                lastJumpTime = Time.time;

            }
            else if (!hasDoubleJumped && timeSinceLastJump > 0.1f)
            { 
                animator.SetBool("doubleJump", true);
                hasDoubleJumped = true;
            }
        }
        Fall();
    }

    private void Fall()
    {
        //High Jump when Jump Key is pressed down
        if (rb.velocity.y < 0)
         {
             animator.SetFloat("yVelocity", rb.velocity.y); //Fall animation transition
             rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier+1) * Time.deltaTime; //increases fall speed
         }
        //LowJump when Jump Key is pressed once
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && !hasDoubleJumped)
         {
             rb.velocity += Vector3.up * Physics.gravity.y * Time.deltaTime;
         }  
    } 
    
    public void DoubleJumpEnd()
    {
        animator.SetBool("doubleJump", false);
        rb.velocity = Vector3.up * (jumpForce - 2);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }
    #endregion

    #region Crouch
    private void HandleCrouching()
    {
        // check if there is movementDirection smth above the player while crouched
        bool hitAbove = groundChecker.CheckCeiling();

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            isSliding = true;
            bool isMoving;
            if (Camera.main.orthographic)
            {
                isMoving = horizontalInput != 0;
            }
            else
            {
                isMoving = horizontalInput != 0 || verticalInput != 0;
            }
            HandleSliding(isMoving);   
        }  
        else
        {
            if (!hitAbove)
            {
                isSliding = false;
              //  walkingSpeed = 5.0f;
                animator.SetBool("isCrouching", false);
                animator.SetBool("isSliding", false);
            }
            
        } 
    }

    void HandleSliding(bool sliding)
    {
        animator.SetBool("isSliding", sliding);
        animator.SetBool("isCrouching", !sliding);
        if (sliding)
        {
            walkingSpeed = 7.0f;
        }
    }
    #endregion
}
