using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] private int maxHealth;
    private Enemy enemy;
     private int health;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        health = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp((float)health / maxHealth, 0, 1);
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Physics.IgnoreLayerCollision(3, 6, true);
            _animator.SetTrigger("Dead");
        }
        else
        {
            _animator.SetTrigger("isHit");
            enemy.Knockback();
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
