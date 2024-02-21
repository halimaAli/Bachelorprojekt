using System.Collections;
using UnityEngine;

public class EnemyMovements : MonoBehaviour
{ 
    [SerializeField]
    private float speed = 1;
    public int direction = 1;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected Animator animator;
    protected bool idle;
    public Vector3 axis;
    public bool active;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idle = false;
        active = true;
        EnemyController controller = GetComponent<EnemyController>();
        controller.movements = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

        MoveLeftandRight(); 
    }

    #region Basic Left and Right Movement
    public void MoveLeftandRight()
    {
        FlipSprite();
        if (Camera.main.orthographic)
        {
            axis = Vector3.left;
        } else
        {
            axis = Vector3.back;
        }
        transform.Translate(axis * speed * direction *  Time.deltaTime);
        animator.SetBool("isWalking", true);
    }
    #endregion

    #region Move to specific position
    public void MoveTo(Vector3 targetPos, bool isAttacking)
    {
        float distance = Vector3.Distance(transform.position, targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, distance * speed * Time.deltaTime);
        animator.SetBool("isAttacking", isAttacking);
        spriteRenderer.flipX = isAttacking;
    }
    #endregion

    private void DisableMovement(bool enable)
    {
        idle = enable;
    }

    private void ChangeDirection()
    {
        direction *= -1;
    }

    private void FlipSprite()
    {
        if (direction < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

   

    public IEnumerator Idle(float duration)
    {
        animator.SetBool("isStanding", true);
        animator.SetBool("isWalking", false);
        DisableMovement(true);
        yield return new WaitForSeconds(duration);
        DisableMovement(false);
        ChangeDirection();
    } 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Wall"))
        {
            ChangeDirection();
        }
    }
}
