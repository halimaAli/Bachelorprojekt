using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] private int maxHealth;
    private Enemy enemy;
     private int health;

    [SerializeField] private AudioClip damageSoundClip;

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
        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1,false);
        if (health <= 0)
        {
            Physics.IgnoreLayerCollision(3, 6, true);
            SoundFXManager.instance.StopLoopingSound(transform);
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
