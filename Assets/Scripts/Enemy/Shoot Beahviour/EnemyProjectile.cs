using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag.Equals("ground") || other.gameObject.tag.Equals("Player"))
        {
            _animator.SetTrigger("Destroyed");
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
