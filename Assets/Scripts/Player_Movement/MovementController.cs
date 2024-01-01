using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MovementController : MonoBehaviour
{
     private float horizontalInput;
     private float verticalInput;
     private float walkingSpeed = 5.0f;
     private float jumpForce = 40.0f;

     [SerializeField] private Vector3 boxSize = new Vector3(1,0.2f,0);
     private float maxDistance = 1;
     [SerializeField] private LayerMask groundMask;
       
     private Rigidbody rb;
     private Animator animator;
     private SpriteRenderer spriteRenderer;
     [SerializeField] private BoxCollider crouchCollider;
     [SerializeField] private BoxCollider standingCollider;

     [SerializeField] private bool isCrouched;
     [SerializeField] private bool IsGrounded;
    


    void Start()
     {
         rb = GetComponent<Rigidbody>();
         animator = GetComponent<Animator>();
         spriteRenderer = GetComponent<SpriteRenderer>();
         isCrouched = false;
         IsGrounded = true;
     }

     void Update()
     {
         horizontalInput = Input.GetAxis("Horizontal"); // A and D Input
         verticalInput = Input.GetAxis("Vertical"); //// W and S Input

         HandleWalking();
         CheckIfGrounded();
         HandleJumping();
         HandleCrouching();
     }

    #region Walk
    private void HandleWalking()
     {
         animator.SetBool("isWalking", Camera.main.orthographic ? horizontalInput != 0 : horizontalInput != 0 || verticalInput != 0);

         if (Input.GetKey(KeyCode.D))
         {
             spriteRenderer.flipX = false;
         }
         else if (Input.GetKey(KeyCode.A))
         {
             spriteRenderer.flipX = true;
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
    }

    #endregion

    #region Jump

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
        }
    }
    #endregion

    #region Crouch
    private void HandleCrouching()
     {
         if (Input.GetKeyDown(KeyCode.LeftShift))
         {
             if (!isCrouched)
             {
                 animator.SetBool("isCrouching", true);
                 standingCollider.enabled = false;

                 crouchCollider.enabled = true;
                 isCrouched = true;
             }
             else
             {
                 animator.SetBool("isCrouching", false);
                 standingCollider.enabled = true;

                 crouchCollider.enabled = false;
                 isCrouched = false;
             }

         }
     }
    #endregion
}
