using Cinemachine;
using UnityEngine;

public class SwitchCameraConfiners2D : MonoBehaviour
{

    [SerializeField] private CinemachineConfiner confiner;
    [SerializeField] private CompositeCollider2D _collider;

    private void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hello");
        if (other.gameObject.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = _collider;
        }
    }
}
