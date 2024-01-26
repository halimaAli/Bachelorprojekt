using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{ 
    
    [SerializeField]
    private float speed = 1;
    public int direction = 1;

    private SpriteRenderer spriteRenderer;
    private Animator animator;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLeftandRight();
        
    }

    #region Left and Right Movement
    public void MoveLeftandRight()
    {
        if (direction < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        transform.Translate(Vector3.left * speed * direction *  Time.deltaTime);
        animator.SetBool("isWalking", true);
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Wall"))
        {
            direction *= -1;
        }
    }
}
