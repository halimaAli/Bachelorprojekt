using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{ 
    
    [SerializeField]
    private int speed = 1;
    private int direction = 1;

    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            spriteRenderer.flipX = true;
        } else
        {
            spriteRenderer.flipX = false;
        }

        transform.Translate(Vector3.right * speed * direction *  Time.deltaTime);
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
