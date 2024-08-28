using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Axis movementAxis;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float threshold = 0.1f;

    private Vector3 direction;
    private int movementDirection = 1;
    private Transform playerTransform;
    private Vector3 lastPlatformPosition;

    private enum Axis
    {
        Horizontal,
        Vertical
    }

    private void Start()
    {
        lastPlatformPosition = transform.position;
    }

    private void Update()
    {
        SetDirection();
        MovePlatform();
        CheckDirectionChange();

        if (playerTransform != null)
        {
            Vector3 platformMovement = transform.position - lastPlatformPosition;
            playerTransform.position += platformMovement;
        }

        lastPlatformPosition = transform.position;
    }

    private void SetDirection()
    {
        if (Camera.main.orthographic)
        {
            direction = movementAxis == Axis.Horizontal ? Vector3.right : Vector3.up;
        }
        else
        {
            direction = movementAxis == Axis.Horizontal ? Vector3.forward : Vector3.up;
        }
    }

    private void MovePlatform()
    {
        transform.Translate(direction * speed * movementDirection * Time.deltaTime);
    }

    private void CheckDirectionChange()
    {
        
        if (Vector3.Distance(transform.position, pointA.position) < threshold)
        {
            movementDirection = 1;
        }
        else if (Vector3.Distance(transform.position, pointB.position) < threshold)
        {
            movementDirection = -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }
}
