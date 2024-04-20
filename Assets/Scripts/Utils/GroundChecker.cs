using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform center;

    public bool IsGrounded { get; private set; }

    public bool CheckGround()
    {
        return Physics.BoxCast(center.position, boxSize, Vector3.down, Quaternion.identity, maxDistance, groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center.position + Vector3.down * maxDistance, boxSize);
    }
}
