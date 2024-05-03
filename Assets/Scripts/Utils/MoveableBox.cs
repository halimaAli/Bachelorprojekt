using UnityEngine;

public class MovableBox : MonoBehaviour
{
    public float pushForce = 5f; 
    private bool isBeingPushed = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Freeze rotation of the box
        rb.constraints |= RigidbodyConstraints.FreezePositionY; // Freeze Y position of the box
    }

    private void FixedUpdate()
    {
        if (isBeingPushed)
        {
            Vector3 pushDirection = CalculatePushDirection();

            rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
        }
    }

    private Vector3 CalculatePushDirection()
    {
        Vector3 pushDirection = transform.position - PlayerController.instance.transform.position;

        if (Camera.main.orthographic)
        {
            pushDirection.z = 0f;
           // rb.constraints |= RigidbodyConstraints.FreezePositionZ; // Freeze Z position of the box in 2D mode
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ; // Unfreeze Z position in 3D mode
        }

        pushDirection.y = 0f;

        return pushDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBeingPushed = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBeingPushed = false;
            rb.velocity = Vector3.zero;
        }
    }
}
