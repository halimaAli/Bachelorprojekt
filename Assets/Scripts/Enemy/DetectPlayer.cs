using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private LayerMask playerDetectionMask;
    private Collider[] playerCollider = new Collider[1];
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float offsetSmoothing = 5.0f;
    private Animator ani;
    private Vector3 startPosition;

    private Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        size = new Vector3(4 * 2, 4 * 2, 1);
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool playerDetected = Physics.OverlapBoxNonAlloc(transform.position, size, playerCollider, new Quaternion(0, 0, 0, 0), playerDetectionMask) > 0;
        if (playerDetected)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, offsetSmoothing * Time.deltaTime);
            ani.SetBool("isAttacking", true);
        } else
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, offsetSmoothing * Time.deltaTime);
            ani.SetBool("isAttacking", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(transform.position, size);
    }
}
