using UnityEngine;


public class CombatController : MonoBehaviour
{

    public GameObject projectilePrefab;
    public Transform positioning;

    public void HandleShooting()
    {
        if (Camera.main.orthographic)
        {
            projectilePrefab.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            projectilePrefab.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        Instantiate(projectilePrefab, positioning.position, projectilePrefab.transform.rotation);    
    }
}
