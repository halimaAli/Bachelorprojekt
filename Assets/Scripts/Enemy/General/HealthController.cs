using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] private int maxHealth;
     private int health;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        health = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp((float)health / maxHealth, 0, 1);
        //print(healthBar.fillAmount + " health: " + health + "(float)health / maxHealth: " + (float)health / maxHealth);
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
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
