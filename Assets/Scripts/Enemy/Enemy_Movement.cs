using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{ 
    
    [SerializeField]
    private float speed = 1;
    public int direction = 1;

    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MoveLeftandRight();
        
    }

    #region Left and Right Movement
    private void MoveLeftandRight()
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
    }
    #endregion

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("ground"))
        {
            direction *= -1;
        }
    }

}
