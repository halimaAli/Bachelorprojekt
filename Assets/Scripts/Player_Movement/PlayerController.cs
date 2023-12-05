using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{

    public float horizontalInput;
    public float speed;

    public Rigidbody rb;
    public float jumpForce = 10.0f;
    public bool isGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
    }


    // Update is called once per frame
    void Update()
    {
        //move left and right
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);

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
