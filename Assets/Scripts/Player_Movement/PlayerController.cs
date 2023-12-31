using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public BoxCollider crouchCollider;
    public BoxCollider standingCollider;

    private bool active = true;

    private Vector3 respawnPoint;


    private void Awake()
    {

        animator = GetComponent<Animator>();
        SetRespawnPoint(transform.position);
    }


    // Update is called once per frame
    void Update()
    {

        if (!active)
        {
            SetRespawnPoint(new Vector3(transform.position.x - 10, transform.position.y, transform.position.z));
            return;
        }

        //check if player is falling from platform
        if (transform.position.y < -6)
        {
            Die(true);
            return;
        }

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
        crouchCollider.enabled = false;
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
        crouchCollider.enabled = true;
        standingCollider.enabled = true;
        animator.SetBool("isMoving", false);
        animator.SetBool("isDead", false);
        MiniJump();
    }
}
