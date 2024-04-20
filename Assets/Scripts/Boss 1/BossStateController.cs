using System;
using System.Collections;
using UnityEngine;


public class BossStateController : MonoBehaviour
{
    public static BossStateController instance;

    public Animator animator;
    public Rigidbody rb;
    public SpriteRenderer spriteRenderer;
    private new BoxCollider collider;
    public GameObject player;

    public bool isAttacking;
    public float localScaleX;


    public int direction;

    [SerializeField] private float minDistance;
    public Vector3 startPos;

    private bool isWaitingForNextAttack = false;
    private int phase;
    public new bool enabled;

    private enum Phases
    {
        SpinAttack,
        SummonAttack,
        JumpAttack,
        ComboAttack
    }

    void Awake()
    {
        if (instance == null) instance = this;
       
        startPos = transform.position;
        phase = -1;
        enabled = true;
    }


    private void Start()
    {
        localScaleX = transform.localScale.x;
       // StartCoroutine(Intro());
    }

    void Update()
    {
        if (enabled)
        {
            FacePlayer();
        }
        
        if (!isAttacking && !isWaitingForNextAttack)
        { 
            StartCoroutine(ChooseRandomPhaseWithDelay());
        }
    }

    private IEnumerator ChooseRandomPhaseWithDelay()
    {
        isWaitingForNextAttack = true; 
                                       
        yield return new WaitForSeconds(2f);
        isWaitingForNextAttack = false; 

        ChooseRandomPhase();
    }

    private void ChooseRandomPhase()
    {
        /*System.Random random = new System.Random();
        int phase = random.Next(3);*/
        isAttacking = true;
        if (phase == 2)
        {
            phase = -1;
        }

        phase++;

        switch (phase)
        {
            case 0:
                 SpinAttack();
                break;
            case 1:
                 ComboAttack();

                break;
            case 2:
                JumpAttack();
                break;
        }
    }

    public void SpinAttack()
    {
        animator.SetTrigger("SpinAttack");
    }

    public void ComboAttack()
    {
        animator.SetTrigger("Combo Attack");
    }

    public void JumpAttack()
    {
         animator.SetTrigger("Jump Attack");
    }

    #region Util
    public void Idle()
    {
        enabled = true;
        animator.SetBool("isWalking", true);  
    }

    public void FacePlayer()
    {
        float distance = player.transform.position.x - transform.position.x;

        if (distance > 0) //Player is on the  LEFT  side of the boss
        {
            direction = 1;
            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }
        else //Player is on the  RIGHT  side of the boss
        {
            direction = -1;
            transform.localScale = new Vector3(-1 * localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    public void Wait(float delay)
    {
        StartCoroutine(Delay(delay));
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    internal void JumpTo(Vector3 position)
    {
        animator.SetBool("isJumping", true);
        rb.AddForce(position, ForceMode.Impulse);
    }
    #endregion

}
