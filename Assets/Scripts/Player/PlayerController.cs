using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private string username;
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
    [SerializeField] private AudioClip deathSound;
    private int coins;
    public bool IsGrounded;

    private string currentLevel;
    internal bool isVulnerable;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        active = true;
        
        color = rend.material.color;
        if (health <= 0)
        {
            health = maxHealth;
        }
        Physics.IgnoreLayerCollision(3, 6, false);
    }

    void Update()
    {
        if (UIHandler.instance.isPaused)
        {
            return;
        }

        if (!active)
        {
            return;
        }

        if (!isVulnerable)
        {
            Physics.IgnoreLayerCollision(3, 6, false);
        }

        direction = rend.flipX? -1 : 1;

        HandleRespawnPoint();

        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp((float)health / maxHealth, 0, 1);
        }

        if (health <= 0 || transform.position.y < -50) // if player falls from map for some reason
        {
            Die();
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

    public void Die()
    {
        active = false;
        SoundFXManager.instance.PlaySoundFXClip(deathSound, transform, 1, false);
        Physics.IgnoreLayerCollision(3, 6, true);
        animator.SetBool("isDead", true);
    }

    public void RestartOrRespawn()
    {
        // Check if Player died in Boss Lvl
        currentLevel = SceneManager.GetActiveScene().buildIndex == 4 ? "Boss" : "";
        if (currentLevel.Equals("Boss"))
        {
            BossLevelManager.instance.Result(false);
        }
        else
        {
            StartCoroutine(Respawn());
        }
    }

    public void TakeDamage()
    {
        if (isVulnerable)
        {
            return;
        }

        health--;
        Knockback();

        if (health <= 0)
        {
            Die();
        }
        else
        {
            active = false;
            animator.SetTrigger("isHit");
            UIHandler.instance.UpdateHealth(health);
            StartCoroutine(BecomeInvulnerable());
        }    
    }

    public void TakeDamageAnimationEnd()
    {
        active = true;
    }

    public void SetRespawnPoint(Vector3 position)
    {
        DataPersistenceManager.Instance.SetLastPosition(position);
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.5f);

        //Set Player Position next to Respawn Point
        transform.position = respawnPoint;
        rb.velocity = Vector3.zero;

        //Enable Input Movement and enable collsions
        active = true;
        Physics.IgnoreLayerCollision(3, 6, false);
        animator.SetBool("isDead", false);

        //Re-Init the Health UI
        health = maxHealth;
        UIHandler.instance.UpdateHealth(health);

        //Only needed during Tutorial Level
        if (TutorialLevelManager.instance != null)
        {
            TutorialLevelManager.instance.CheckIfAttempted();
        }
    }

    private IEnumerator BecomeInvulnerable()
    {
        isVulnerable = true;
        Physics.IgnoreLayerCollision(3, 6, true);
        color.a = 0.5f;
        rend.material.color = color;
        yield return new WaitForSeconds(2f);
        Physics.IgnoreLayerCollision(3, 6, false);
        color.a = 1f;
        rend.material.color = color;
        isVulnerable = false;
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
        coins = data.coins;
        health = data.healthPoints;
        respawnPoint = data.spawnPoint;
        username = data.username;
        
        UIHandler.instance.InitializeUI(username, coins, health);

        SetRespawnPoint(respawnPoint);
        transform.position = respawnPoint;

        switch (data.currentSceneIndex)
        {
            case 1:
                currentLevel = "Tutorial";
                break;
            case 2:
                currentLevel = "Level";
                break;
            case 3: currentLevel = "Boss";
                break;
        }
    }

    public void SaveData(GameData data)
    {
        data.coins = this.coins;
        data.healthPoints = this.health;
        data.spawnPoint = this.respawnPoint;
    }
}
