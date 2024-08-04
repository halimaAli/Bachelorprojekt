using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private Axis axis;
    [SerializeField] private int speed;
    [SerializeField] private Transform pointA, pointB;
    private Vector3 direction;
    private int movementDirection;
    private float threshold = 2.0f;
    private enum Axis
    {
        Horizontal,
        Vertical
    }

    // Start is called before the first frame update
    void Start()
    {
        if (axis == Axis.Horizontal)
        {
            direction = Vector3.right;
        }
        else if (axis == Axis.Vertical)
        {
            direction = Vector3.up;
        }
        movementDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, pointA.position) < threshold)
        {
            movementDirection = 1;
        }
        else if (Vector3.Distance(transform.position, pointB.position) < threshold)
        {
            movementDirection = -1;
        }

        transform.Translate(movementDirection * direction * speed * Time.deltaTime);
    }
}
