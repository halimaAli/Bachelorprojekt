using UnityEngine;


public class CombatController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectilePosition;

    public void ShootProjectile()
    {
        if (Camera.main.orthographic)
        {
            projectilePrefab.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            projectilePrefab.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        Instantiate(projectilePrefab, projectilePosition.position, projectilePrefab.transform.rotation);    
    }

    public void MeleeAttack()
    {
        //TODO: Melee Attack Code Here

    }
}
