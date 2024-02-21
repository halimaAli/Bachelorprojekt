using UnityEngine;


public class CombatController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectilePosition;
    public bool active = true;

    private void Start()
    {
        EnemyController controller = GetComponent<EnemyController>();
        controller.combat = this;
    }

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
