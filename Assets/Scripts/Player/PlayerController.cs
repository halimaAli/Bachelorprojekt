using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public static PlayerController instance;
    Animator animator;
    public BoxCollider standingCollider;

    private Rigidbody rb;

    private Vector3 playerSize = new Vector3(1.5f, 2.3f, 1);
    [SerializeField] Transform center;

    public bool active {get; set;}
    public int direction { get; set; }

    private Vector3 respawnPoint;
    [SerializeField] private LayerMask respawnPointMask;
    private Collider[] respawnPointCollider = new Collider[1];
    public int health;

    private SpriteRenderer rend;
    private Color color;

    [SerializeField] private int knockback = 900;
    private int maxHealth = 10;
    [SerializeField] private Image healthBar;

    [SerializeField] private AudioClip _2DTo3D;
    [SerializeField] private AudioClip _3DTo2D;
    private int coins;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        active = true;
        
        color = rend.material.color;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

        direction = rend.flipX? -1 : 1;

        HandleRespawnPoint();
        healthBar.fillAmount = Mathf.Clamp((float)health / maxHealth, 0, 1);

        //check if player is falling from platform
        if (transform.position.y < -6)
        {
          //  Die(true);
            return;
        }
        
    }

    public void PlaySwitchAnimation(bool _3D)
    {
        animator.SetTrigger("Switch");
        if (_3D) { 
            SoundFXManager.instance.PlaySoundFXClip(_2DTo3D, transform, 1, false); 
        } else
        {
            SoundFXManager.instance.PlaySoundFXClip(_3DTo2D, transform, 1, false);
        }
        
    }

    private void HandleRespawnPoint()
    {
        bool hitRespawnPoint = Physics.OverlapBoxNonAlloc(center.position, new Vector3(playerSize.x / 2, playerSize.y / 2, playerSize.z / 2), respawnPointCollider, new Quaternion(0, 0, 0, 0), respawnPointMask) > 0;
        if (hitRespawnPoint)
        {
            SetRespawnPoint(new Vector3(respawnPointCollider[0].transform.position.x + 1.5f, transform.position.y, transform.position.z));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //draws movementDirection sphere at the feet of player; helpful in scene view
        Gizmos.DrawWireCube(center.position, playerSize);
    }

    private void MiniJump()
    {
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, 1, transform.position.z), transform.position, Time.deltaTime);
    }

    public void Knockback()
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
        StartCoroutine(Respawn());
    }

    public void TakeDamage()
    {
        health--;
        Knockback();

        if (health <= 0)
        {
            Die(false);
        }
        else
        {
            active = false;
            animator.SetTrigger("isHit");
        }    
    }

    public void TakeDamageAnimationEnd()
    {
        UIHandler.instance.UpdateHealth(health);
        active = true;
        StartCoroutine(BecomeInvulnerable());
    }

    public void SetRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);

        //Set Player Position next to Respawn Point
        transform.position = respawnPoint;
        rb.velocity = Vector3.zero;

        //Enable Input Movement and enable collsions
        active = true;
        Physics.IgnoreLayerCollision(3, 6, false);
        animator.SetBool("isDead", false);

        //Re-Init the Health UI
        UIHandler.instance.UpdateHealth(maxHealth);

        //Only needed during Tutorial Level
        if (TutorialLevelManager.instance != null)
        {
            TutorialLevelManager.instance.attempts++;
        }
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
        if (other.gameObject.tag.Equals("ThirdRespawnPoint"))
        {
            if (!Camera.main.orthographic)
            {
                CameraManager.instance.Set2DView();
            }
        }
        if (other.gameObject.tag.Equals("Coin"))
        {
            coins++;
            UIHandler.instance.UpdateCoins(coins);
        }

        if (other.CompareTag("Health Point"))
        {
            if (!(health + 1 > maxHealth))
            {
                health++;
                UIHandler.instance.UpdateHealth(health);
                Destroy(other.gameObject);
            }
        }
    }

    public void LoadData(GameData data)
    {
        this.coins = data.coins;
        this.health = data.healthPoints;
        this.respawnPoint = data.spawnPoint;
        UIHandler.instance.InitializeUI(coins, health);

        SetRespawnPoint(respawnPoint);
        transform.position = respawnPoint;
    }

    public void SaveData(GameData data)
    {
        data.coins = this.coins;
        data.healthPoints = this.health;
        data.spawnPoint = this.respawnPoint;
    }
}
