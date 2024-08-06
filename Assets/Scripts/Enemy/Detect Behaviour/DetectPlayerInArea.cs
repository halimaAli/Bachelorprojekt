using UnityEngine;

public class DetectPlayerInArea : MonoBehaviour
{
    [SerializeField] private DetectAndChase enemy;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag.Equals("Player"))
        {
            enemy.DetectedInArea(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            enemy.DetectedInArea(false);
        }
    }
}
