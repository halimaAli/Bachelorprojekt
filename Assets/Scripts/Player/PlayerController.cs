using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Animator animator;
    public BoxCollider standingCollider;

    private Rigidbody rb;

    private Vector3 playerSize = new Vector3(1.5f, 2.3f, 1);

    public bool active {get; set;}
    public int direction { get; set; }

    private Vector3 respawnPoint;
    [SerializeField] private LayerMask respawnPointMask;
    private Collider[] respawnPointCollider = new Collider[1];
    private int health;

    private SpriteRenderer rend;
    private Color color;

    private int knockback = 1000;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        SetRespawnPoint(transform.position);
        active = true;
        health = 2;
        color = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

        HandleRespawnPoint();

        //check if player is falling from platform
        if (transform.position.y < -6)
        {
            Die(true);
            return;
        }
    }

    private void HandleRespawnPoint()
    {
        bool hitRespawnPoint = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(playerSize.x / 2, playerSize.y / 2, playerSize.z / 2), respawnPointCollider, new Quaternion(0, 0, 0, 0), respawnPointMask) > 0;
        if (hitRespawnPoint)
        {
            SetRespawnPoint(new Vector3(respawnPointCollider[0].transform.position.x + 1.5f, transform.position.y, transform.position.z));

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //draws a sphere at the feet of player; helpful in scene view
        Gizmos.DrawWireCube(transform.position, playerSize);
    }

    private void MiniJump()
    {
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, 1, transform.position.z), transform.position, 1 * Time.deltaTime);
    }

    private void FallBack()
    {
        if (rend.flipX) //if Player if facing left -> knockback to the right
        {
            rb.AddForce(new Vector3(1, 1, 0) * knockback);
        }
        else //if Player if facing right -> knockback to the left
        {
            rb.AddForce(new Vector3(-1, 1, 0) * knockback);
        }
    }


    public void Die(bool falling)
    {
        active = false;
        Physics.IgnoreLayerCollision(3, 6, true);
        animator.SetBool("isDead", true);
        if (!falling)
        {
           FallBack();
        }
        StartCoroutine(Respawn());
    }

    public void TakeDamage()
    {
        health--;
        if (health == 0)
        {
            Die(false);
        }
        else
        {
            active = false;
            animator.SetBool("isHurt", true);
        }
    }

    public void TakeDamageAnimationEnd()
    {
        animator.SetBool("isHurt", false);
        UIHandler.instance.updateHP(health);
        active = true;
        StartCoroutine(BecomeInvulnerable());
    }

    public void SetRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.5f);
        //Set Player Position next to Respawn Point
        transform.position = respawnPoint;

        //Enable Input Movement and enable collsions
        active = true;
        Physics.IgnoreLayerCollision(3, 6, false);
        animator.SetBool("isDead", false);

        //Re-Init the Health UI
        health = 2;
        UIHandler.instance.updateHP(health);
        MiniJump();
        //LevelManager.instance.MinusOneLife();
    }

    private IEnumerator BecomeInvulnerable()
    {
        Physics.IgnoreLayerCollision(3, 6, true);
        color.a = 0.5f;
        rend.material.color = color;
        yield return new WaitForSeconds(3f);
        Physics.IgnoreLayerCollision(3, 6, false);
        color.a = 1f;
        rend.material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Coin"))
        {
            UIHandler.instance.onCoinCollected();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag.Equals("ThirdRespawnPoint"))
        {
           // LevelManager.instance.phase = 4;
            if (!Camera.main.orthographic)
            {
                CameraManager.instance.Set2DView();
            }
        }
    }
}
