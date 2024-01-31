using UnityEngine;

public class DeleteEnemy : MonoBehaviour
{
    [SerializeField] FlyingEyeController[] controllers;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag.Equals("Enemy"))
        {
            Destroy(other.gameObject);
        } 
        
        if (other.gameObject.tag.Equals("Player"))
        {
            foreach (var controller in controllers)
            {
                controller.playerDetected = true;
            }
        }
    }
}
