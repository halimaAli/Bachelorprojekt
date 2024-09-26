using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // These Value have to be manually set for each projectile
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float lifeTime = 1.0f;
    [SerializeField] private Type type;
    [SerializeField] private Angle angle;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private float direction = 1.0f;
    public Vector3 target;

    [SerializeField] private bool canControl;

    public enum Type
    {
        Player,
        Enemy,
        Archer,
        Boss,
        Boss_Hammer
    }

    public enum Angle
    {
        Right,
        Up
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDirection();
        SetAngle();
    }

    private void SetAngle()
    {
        switch (angle)
        {
            case Angle.Right:
                target = Vector3.right * direction;
                break;
            case Angle.Up:
                target = Vector3.right * direction + Vector3.up;
                break;
        }
    }

    private void SetDirection()
    {
        switch (type)
        {
            case Type.Player:
                direction = PlayerController.instance.direction;
                break;
            case Type.Boss:
                direction = BossStateController.instance.direction;
                break;
        }
    }

    void Update()
    {
        if (Type.Enemy == type) return;


        if (Camera.main.orthographic)
        {
            HandleMovementIn2D();
        }
        else
        {
            HandleMovementIn3D();
        }
        DestroyProjectile();
    }

    private void HandleMovementIn2D()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.Translate(target * Time.deltaTime * speed);
        FlipProjectile();
    }

    private void HandleMovementIn3D() //TO DO: Rework for Mini bosses (see notes)
    {
        if (type == Type.Player || type == Type.Boss)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.Translate(Vector3.forward * direction * Time.deltaTime * speed);
        }
        else //Arrows of Archer have to be rotated and move towards X
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        
    }

    private void FlipProjectile()
    {
        spriteRenderer.flipX = direction < 0;
    }

    private void DestroyProjectile()
    {
        if (!spriteRenderer.isVisible && type == Type.Player)
        {
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
    }
}
