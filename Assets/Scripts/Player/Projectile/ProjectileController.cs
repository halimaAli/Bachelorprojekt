using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Range(1, 20), SerializeField] private float speed = 20.0f;
    [SerializeField] private float lifeTime = 1.0f;
    [SerializeField] private float direction = 1.0f;
    [SerializeField] private bool isPlayer;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isPlayer)
        {
            direction = PlayerController.instance.direction;
        }
    }

    void Update()
    {
        if (Camera.main.orthographic)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed * direction);
        } else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed * direction);
        }

        if (!spriteRenderer.isVisible)
        {
            Destroy(gameObject);
            return;
        }

        //else destroy after 3 secs
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy if it hits wall or ground
        if (other.gameObject.tag.Equals("ground"))
        {
            Destroy(gameObject);
        }
    }
}