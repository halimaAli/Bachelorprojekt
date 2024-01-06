using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    private int speed = 1;
    [SerializeField] private int jumpForce = 3;

    private int direction = 1;
    private bool isGrounded = true;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();

    }

    #region Jump
    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    #endregion

    #region Left and Right Movement
    private void MoveLeftandRight()
    {
        transform.Translate(Vector3.right * speed * direction *  Time.deltaTime);
    }
 #endregion
  
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("ground"))
        {
            direction *= -1;
            isGrounded = true;
        } 
    }
}
