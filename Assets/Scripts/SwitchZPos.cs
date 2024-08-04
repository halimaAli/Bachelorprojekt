using UnityEngine;

public class SwitchZPos : MonoBehaviour
{
    [SerializeField] private RotationController rotationController;
    [SerializeField] private float zPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {

            rotationController.SetNewZPos2D(zPos);

        }
    }
}
