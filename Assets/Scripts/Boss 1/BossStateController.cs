using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class BossStateController : MonoBehaviour
{
    public static BossStateController instance;

    public Animator animator;
    public Rigidbody rb;
    public SpriteRenderer spriteRenderer;
    private new BoxCollider collider;
    private JumpAttackState jumpAttackState;
    private SpinAttackState spinAttackState;
    private ComboAttackState comboAttackState;
    private State currentState;
    public GameObject player;
    public GameObject boss;

    public bool isAttacking;
    private float localScaleX;

    public GameObject[] slashes = new GameObject[3];
    public Transform[] comboPos = new Transform[3];
    public int direction;

    private enum Phases
    {
        

    }

    void Awake()
    {
        if (instance == null) instance = this;

        jumpAttackState = new JumpAttackState(this);
        spinAttackState = new SpinAttackState(this);
        comboAttackState = new ComboAttackState(this);

        boss = gameObject;

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }


    private void Start()
    {
        localScaleX = transform.localScale.x;

        FacePlayer();
        currentState = jumpAttackState;
        currentState.Enter();  
    }

    void Update()
    {
        FacePlayer();
        if (currentState == null)
        {
           // animator.SetBool("isIdle", true);
        } else
        {
            currentState.UpdateState();
        }
    }

    public void Init()
    {  
        StartCoroutine(Intro());
    }

    public IEnumerator Intro()
    {
        animator.SetTrigger("Intro");
        rb.AddForce(new Vector3(20, 3, 0), ForceMode.Impulse); //Placeholder values; must be changed in final Boss Arena


        yield return new WaitForSeconds(10);

        currentState = comboAttackState;
        currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        if (currentState != null)
        {
            currentState.Enter();
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


    //------------- Attack Methods -------------//
    public void StartSpinAttack() //is called in the middle of the SpinAttack Animation
    {
        isAttacking = true;
        int speed;

        if (transform.localScale.x > 0)
        {
            speed = 20;
        } else
        {
            speed = -20;
        }

        rb.AddForce(new Vector3(speed, 0, 0), ForceMode.Impulse);
        Wait(1);
    }

    public void EndAttack()
    {
        isAttacking = false;
       // ChangeState(null);
        animator.SetBool("isIdle", true);
        print("End attack");
    }

    public void ChangePos()
    {
        transform.position = new Vector3(transform.position.x + (direction * 11), transform.position.y, transform.position.z);
        EndAttack();
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

    public void ComboAttackPhases(int phase)
    {
        slashes[phase].transform.localScale = comboPos[phase].transform.localScale;
        Instantiate(slashes[phase], comboPos[phase].position, slashes[phase].transform.rotation);
    }


}
