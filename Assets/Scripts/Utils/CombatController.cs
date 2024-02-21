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
        if (!active)
        {
            return;
        }

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
}
