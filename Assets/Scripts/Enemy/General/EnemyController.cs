using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator animator;
    public int health;
    public EnemyMovements movements;
    public CombatController combat;
    public MushroomController mushroom;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movements = GetComponent<EnemyMovements>();
        combat = GetComponent<CombatController>();
        mushroom = GetComponent<MushroomController>();
    }

    public void TakeDamage()
    {
        health--;
        if (health == 0 ) {
            if (movements != null)
            {
                movements.active = false;
            } 
            else if (combat != null)
            {
                combat.active = false;
            } else if (mushroom != null)
            {
                mushroom.active = false;
            }

            animator.SetBool("isDead", true);
        } else
        {
            animator.SetBool("isHit", true);
        }
    }

    public void DyingAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void HitAnimationEnd()
    {
        animator.SetBool("isHit", false);
    }

    public void FlyingEnemyFall()
    {

    }

}
