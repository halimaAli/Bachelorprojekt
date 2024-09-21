using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGuardian : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform door;
    [SerializeField] private float followDistance = 5f;
    [SerializeField] private float maxDist = 10f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float doorThreshold = 0.5f;

    private bool isWaiting = false;
    private bool canMove = false;
    private Animator m_Animator;
    private TGDialog m_Dialog;
    private bool hasReachedDoor = false;
    private bool hasStartedSecondDialogue = false;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Dialog = GetComponent<TGDialog>();
    }

    void Update()
    {
        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (canMove && !hasReachedDoor && !isWaiting)
        {
            // Set Maximal Distance between player and TM before TM stops moving
            if (distance >= maxDist)
            {
                isWaiting = true; 
            }  
            else
            {
                MoveTowardsDoor();
            }
        } 
        
        else
        {
            if (hasReachedDoor)
            {
                if (!hasStartedSecondDialogue)
                {
                    m_Dialog.StartSecondDialogue();
                    hasStartedSecondDialogue = true;
                }
            }
            if (isWaiting)
            {
                // Check if player is closed enough to move again
                if (distance <= followDistance)
                {
                    isWaiting = false;
                }
            } 
            else if (canMove)
            {
                canMove = false;
            }
            

            m_Animator.SetBool("isWalking", false);
            FacePlayer();
        }
    }

    void MoveTowardsDoor()
    {
        Vector3 direction = door.position - transform.position;

        if (direction.x <= doorThreshold)
        {
            hasReachedDoor = true;
            return;
        }

        m_Animator.SetBool("isWalking", true);

        if (direction.x > 0 && transform.localScale.x < 0)
        {
            Flip(); 
        }
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        direction.y = 0;
        direction.z = 0;
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    void FacePlayer()
    {
        float direction = player.position.x - transform.position.x;

        if (direction > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (direction < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void EnableMovement(bool enable) // TODO: Call from other script when TM is done with dialog
    {
        canMove = enable;
    }
}
