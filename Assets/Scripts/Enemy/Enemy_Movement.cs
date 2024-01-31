using System;
using System.Collections;
using UnityEngine;

public class EnemyMovements : MonoBehaviour
{ 
    [SerializeField]
    private float speed = 1;
    public int direction = 1;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    protected Animator animator;
    protected bool idle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idle = false;
    }

    // Update is called once per frame
    void Update()
    {
       // MoveLeftandRight();
        
    }

    #region Basic Left and Right Movement
    public void MoveLeftandRight()
    {
       // if (idle) { return; }

        FlipSprite();
        //Move forward in the direction of movement
        transform.Translate(Vector3.left * speed * direction *  Time.deltaTime);
        animator.SetBool("isWalking", true);
        animator.SetBool("isStanding", false);
    }

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

    #endregion

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
