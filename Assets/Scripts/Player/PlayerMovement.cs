using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider boxCollider;
    private RotationController rotationController;

    [Header("Movement")]
    public float horizontalInput;
    private float verticalInput;
    public float walkingSpeed;
    [SerializeField] private bool isHorizontalMovementOnly;

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

    [Header("FX")]
    [SerializeField] private ParticleSystem dust;
    [SerializeField] AudioClip jumpingSound;
    [SerializeField] AudioClip landingSound;
    [SerializeField] AudioSource walkingSound;
    [SerializeField] private PlayerFXController playerFXController;
    

    void Start()
     {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        groundChecker = GetComponent<GroundChecker>();
        rotationController = GetComponent<RotationController>();
        isGrounded = true;
    }

    void Update()
     {
        if (UIHandler.instance.isPaused)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal"); // A and D Input
        verticalInput = Input.GetAxis("Vertical"); //// W and S Input
        if (!PlayerController.instance.active)
        {
            animator.SetBool("isWalking", false);
            playerFXController.Walking = false;
            return;
        }
        HandleWalking();

        if (isHorizontalMovementOnly)  return; 

        CheckIfGrounded();
        HandleJumping();
        HandleCrouching();
        PlayerController.instance.IsGrounded = isGrounded;

        if (Camera.main.orthographic )
        {
            if (transform.position.z != rotationController.defaultZPos2D)
            { 
                transform.position = new Vector3(transform.position.x, transform.position.y, rotationController.defaultZPos2D);
            }
        }
    }

    #region Walk
    private void HandleWalking()
     {
        animator.SetBool("isWalking", Camera.main.orthographic? horizontalInput != 0 : horizontalInput != 0 || verticalInput != 0);

        int previousDirection = PlayerController.instance.direction;

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

            if (horizontalInput != 0 && isGrounded)
            {
                playerFXController.Walking = true;

            } 
            else if (horizontalInput != 0 && !isGrounded)
            {
                playerFXController.Walking = false;
            }
            else if (horizontalInput == 0 && isGrounded)
            {
                playerFXController.Walking = false;

            } else if (horizontalInput == 0 && !isGrounded)
            {
                playerFXController.Walking = false;
            }

            if (previousDirection != PlayerController.instance.direction)
            {
                CreateDust();
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
            SoundFXManager.instance.PlaySoundFXClip(landingSound, transform, 1, false);
            CreateDust() ;
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
                CreateDust();
                SoundFXManager.instance.PlaySoundFXClip(jumpingSound, transform, 1, false);
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
            playerFXController.Walking = false;
            playerFXController.Sliding = true;
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
                walkingSpeed = 7.5f;
                animator.SetBool("isCrouching", false);
                animator.SetBool("isSliding", false);
                playerFXController.Sliding = false;
            }
            
        } 
    }

    void HandleSliding(bool sliding)
    {
        animator.SetBool("isSliding", sliding);
        animator.SetBool("isCrouching", !sliding);
        if (sliding)
        {
            walkingSpeed = 10.0f;
        }
    }
    #endregion

    void CreateDust()
    {
        dust.Play();
    }
}
