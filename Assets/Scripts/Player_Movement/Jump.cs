using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Jump : MonoBehaviour
{
    public Rigidbody rb;
    bool isGrounded = true;
    public float jumpForce = 10.0f;
     
     // Start is called before the first frame update
     void Start()
     {
         rb = GetComponent<Rigidbody>();
         
     }

     // Update is called once per frame
     void Update()
     {
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
