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

        //changes animation
        if (horizontalInput == 0 )
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        if (Camera.main.orthographic)
        {
            if (horizontalInput < 0)
            {
                spriteRenderer.flipX = true;
            } else
            {
                spriteRenderer.flipX = false;
            }

            transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
        }
        else
        {
            verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.back * speed * horizontalInput * Time.deltaTime); //TODO: change scene so i can use the right vectors
            transform.Translate(Vector3.right * speed * verticalInput * Time.deltaTime);
        }
       

        //Jump
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ground"))
        {
            isGrounded = true;
        }
    }
}
