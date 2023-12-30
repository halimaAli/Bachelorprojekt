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
    public bool isCrouched;

    Animator animator;
    SpriteRenderer spriteRenderer;
    public BoxCollider crouchCollider;
    public BoxCollider standingCollider;

    private bool active = true;

    private Vector3 respawnPoint;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isGrounded = true;
        isCrouched = false;
        SetRespawnPoint(transform.position);
    }


    // Update is called once per frame
    void Update()
    {

        if (!active)
        {
            SetRespawnPoint(new Vector3(transform.position.x - 10, transform.position.y, transform.position.z));
            return;
        }

        //check if player is falling from platform
        if (transform.position.y < -6)
        {
            Die(true);
            return;
        }
        
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

        //check if player fell off plattform
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
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            if (!isCrouched)
            {
                animator.SetBool("isCrouching", true);
                standingCollider.enabled = false;

                crouchCollider.enabled = true;
                isCrouched = true;
            } else
            {
                animator.SetBool("isCrouching", false);
                standingCollider.enabled = true;

                crouchCollider.enabled = false;
                isCrouched = false;
            }
            
        }
    }

    private void MiniJump()
    {
        //  rb.velocity = new Vector3(rb.velocity.x, 1, rb.velocity.z);
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, 1, transform.position.z), transform.position, 1 * Time.deltaTime);
    }


    public void Die(bool falling)
    {
        active = false;
        standingCollider.enabled = false;
        crouchCollider.enabled = false;
        animator.SetBool("isDead", true);
        if (!falling)
        {
            MiniJump();
        }
        StartCoroutine(Respawn());
    } 

    public void SetRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        transform.position = respawnPoint;
        active = true;
        crouchCollider.enabled = true;
        standingCollider.enabled = true;
        animator.SetBool("isMoving", false);
        animator.SetBool("isDead", false);
        MiniJump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }

        if  (other.gameObject.tag.Equals("Enemy"))
        {
          /*  animator.SetBool("isDead", true);
            standingCollider.enabled = false;
            crouchCollider.enabled = false;*/
            
        }
    }
}
