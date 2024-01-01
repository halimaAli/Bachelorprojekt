using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public BoxCollider standingCollider;

    private Vector3 playerSize = new Vector3(1.25f, 2.25f, 1);

    private bool active = true;
    public bool is2D;

    private Vector3 respawnPoint;
    [SerializeField] private LayerMask respawnPointMask;
    private Collider[] respawnPointCollider = new Collider[1];


    private void Start()
    {
        animator = GetComponent<Animator>();
        SetRespawnPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        is2D = Camera.main.orthographic;
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
        //  rb.velocity = new Vector3(rb.velocity.x, 1, rb.velocity.z);
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, 1, transform.position.z), transform.position, 1 * Time.deltaTime);
    }

    public void Die(bool falling)
    {
        active = false;
        standingCollider.enabled = false;
        animator.SetBool("isDead", true);
        if (!falling)
        {
            MiniJump();
        }
        StartCoroutine(Respawn());
    } 

    public void SetRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        transform.position = respawnPoint;
        active = true;
        standingCollider.enabled = true;
        animator.SetBool("isDead", false);
        MiniJump();
    }
}
