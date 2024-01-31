using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider boxCollider;

    [Header("Movement")]
    private float horizontalInput;
    private float verticalInput;
    private float walkingSpeed = 5.0f;

    [Header("Jumping")]
    public float jumpForce = 40.0f;
    public float fallMultiplier = 1.5f;
    public float lowJumpMultiplier = 2f;

    [Header("GroundCheck")]
    [SerializeField] private Vector3 boxSize = new Vector3(1.2f,0.2f,0);
    private float maxDistance = 1;
    [SerializeField] private LayerMask groundMask;
    private bool IsGrounded;

    [Header("Crouching")]
    private Vector3 crouchedCenter;
    private Vector3 crouchedSize;
    private Vector3 standingCenter;
    private Vector3 standingSize;

    void Start()
     {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        IsGrounded = true;

        //Set Colliders stats for normal and crouched state
        standingCenter = boxCollider.center;
        crouchedCenter = new Vector3(standingCenter.x, -0.04298978f, standingCenter.z);

        standingSize = boxCollider.size;
        crouchedSize = new Vector3(standingSize.x, 0.2340206f, standingSize.z);
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

        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            PlayerController.instance.direction = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            PlayerController.instance.direction = -1;
        }

        if (Camera.main.orthographic)
        {
            transform.Translate(Vector3.right * walkingSpeed * horizontalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * walkingSpeed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.forward * walkingSpeed * verticalInput * Time.deltaTime);
        }
     }
    #endregion

    #region GroundCheck

    private void CheckIfGrounded()
    {

        // bool grounded = Physics.OverlapSphereNonAlloc(transform.position + new Vector3(0, grounderOffset), grounderRadius, _ground, groundMask) > 0;// allowed wall jumping

         bool grounded = Physics.BoxCast(transform.position, boxSize, Vector3.down, new Quaternion(0, 0, 0, 0), maxDistance, groundMask); 

        if (!IsGrounded && grounded)
        {
            IsGrounded = true;
            animator.SetBool("isJumping", false);
        }
        else if (IsGrounded && !grounded) 
        {
            IsGrounded = false;
            transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireCube(transform.position + Vector3.down * maxDistance, boxSize);
        Gizmos.DrawWireCube(transform.position + Vector3.up * maxDistance, boxSize);
    }
    #endregion

    #region Jump

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.velocity = Vector3.up * jumpForce;
        }

        //High Jump when  Jump Key is pressed down
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime; //increases fall speed
        }
        //LowJump when Jump Key is pressed once
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * Time.deltaTime;
        }
    }
    #endregion

    #region Crouch
    private void HandleCrouching()
    {
        // check if there is a smth above the player while crouched
       bool hitAbove = Physics.BoxCast(transform.position, boxSize, Vector3.up, new Quaternion(0, 0, 0, 0), maxDistance);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isCrouching", true);
            boxCollider.size = crouchedSize;
            boxCollider.center = crouchedCenter;
        }  
        else
        {
            if (!hitAbove)
            {
                animator.SetBool("isCrouching", false);
                boxCollider.size = standingSize;
                boxCollider.center = standingCenter;
            }
            
        } 
    }
    #endregion
}
