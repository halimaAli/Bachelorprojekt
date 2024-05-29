using UnityEngine;

//Controls the Shooting Attacks
public class CombatController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectilePosition;
    public bool active = true;

    public void ShootProjectile()
    {
        // if Archer dies, movement is disabled
        if (!active)
        {
            return;
        }
        Instantiate(projectilePrefab, projectilePosition.position, projectilePrefab.transform.rotation);    
    }
}
