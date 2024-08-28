using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    private Rigidbody _rb;

    [Header("Settings")]
    [SerializeField] protected int speed;
    protected Transform player;

    protected Vector3 originalPosition;
    protected float distance;
    protected int direction = 1; // facing direction
    protected float threshold = .4f;
    protected bool attacking;
    protected bool detectedPlayer;
    protected float range;
    private float knockback = 200;

    [SerializeField] private AudioClip movingSoundClip;


    public virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalPosition = transform.position;

        if (movingSoundClip != null)
        {
            SoundFXManager.instance.PlayLoopingSound(movingSoundClip, transform, 1);
        }
    }

    protected virtual void Update()
    {
        distance = Vector3.Distance(transform.position, player.position + new Vector3(0, 1, 0));
    }

    public void FaceDirection(Vector3 pos)
    {
        // Determine the direction to face the player
        if (pos.x > transform.position.x && direction < 0)
        {
            Flip();
        }
        else if (pos.x < transform.position.x && direction > 0)
        {
            Flip();
        }
    }

    #region Basic Movement
    protected virtual void Move()
    {
        Vector3 axis = Camera.main.orthographic ? Vector3.right : Vector3.back;
        transform.Translate(axis * speed * direction * Time.deltaTime);
        _animator.SetBool("isWalking", true); 
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (attacking)
        {
            return;
        }
        FaceDirection(player.position);
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        _animator.SetBool("isWalking", true);
    }

    protected virtual bool ReturnToOriginalPosition()
    {
        if (attacking)
        {
            return false;
        }
        FaceDirection(originalPosition);
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
        return Vector3.Distance(transform.position, originalPosition) < threshold;
    }

    public void StopMovement()
    {
        _animator.SetBool("isWalking", false);
    }

    protected virtual void Idle()
    {
        _animator.SetBool("isWalking", false);
    }

    public void Knockback()
    {
        attacking = false;
        Vector3 knockbackDirection = _spriteRenderer.flipX ? Vector3.right : Vector3.left;
        _rb.AddForce((knockbackDirection / 2 + Vector3.up / 2) * knockback);
    }

    protected void Flip()
    {
        direction *= -1;
        _spriteRenderer.flipX = direction < 0;
    }
    #endregion

    #region Attack Player

    protected virtual void CheckAttack(float attackRange)
    {
        range = attackRange;
        if (!attacking && distance <= attackRange)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        attacking = true;

        _animator.SetTrigger("Attack");
    }

    public void CheckIfPlayerWasHit()
    {
        distance = Vector3.Distance(player.position + new Vector3(0, 1, 0), transform.position);
        if (distance <= range + threshold)
        {
            PlayerController.instance.TakeDamage();
        }

        attacking = false;
    }
    #endregion

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Flip();
        }
    }
}
