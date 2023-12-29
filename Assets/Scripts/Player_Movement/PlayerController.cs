using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float horizontalInput;
    public float verticalInput;
    public float speed;

    public Rigidbody rb;
    public float jumpForce = 10.0f;
    public bool isGrounded;

    Animator animator;
    SpriteRenderer spriteRenderer;
    public BoxCollider crouchCollider;
    public BoxCollider standingCollider;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isGrounded = true;
    }


    // Update is called once per frame
    void Update()
    {
        //move left and right
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        TriggerWalkAnimation(Camera.main.orthographic);

        if (Camera.main.orthographic)
        {
            transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);
        }


        //Jump
        Jump();
        Crouch();
    }

    private void TriggerWalkAnimation(bool is2D)
    {
        animator.SetBool("isMoving", is2D ? horizontalInput != 0 : horizontalInput != 0 || verticalInput != 0);


        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void Crouch() 
    {
        if (Camera.main.orthographic)
        {
            if (verticalInput < 0)
            {
                animator.SetBool("isCrouching", true);
                standingCollider.enabled = false;

                crouchCollider.enabled = true;

            }
            else if (verticalInput == 0) //TODO:  Bug: animation stops when player stops pressing S key down
            {
                animator.SetBool("isCrouching", false);
                standingCollider.enabled = true;

                crouchCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }
}
