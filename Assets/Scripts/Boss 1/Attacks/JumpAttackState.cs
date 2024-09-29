using System;
using System.Collections;
using UnityEngine;

public class JumpAttackState : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private GroundChecker groundChecker;

    [Header("Attack Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;

    [Header("References")]
    public GameObject player;
    [SerializeField] private GameObject hammer;
    [SerializeField] private Transform hammerPos;
    [SerializeField] AudioClip jumpUpSoundClip;
    [SerializeField] AudioClip swordSoundClip;

    private bool isDashing;
    public Vector3 direction;
    public bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundChecker = GetComponent<GroundChecker>();
    }

    private void Update()
    {
        isGrounded = groundChecker.CheckGround();

        if (isDashing)
        {
            HandleDash();
        }
    }

    private void HandleDash()
    {
        if (!isGrounded)
        {
            Vector3 landingPosition = direction;
            landingPosition.x += (5 * BossStateController.instance.direction);
            animator.speed = 0;
            transform.Translate(landingPosition * dashForce * Time.deltaTime);
        }
        else
        {
            animator.speed = 1;
            isDashing = false;
        }
    }

    public void OnStartJumpAttack()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        SoundFXManager.instance.PlaySoundFXClip(jumpUpSoundClip, transform, 1, false);
    }

    public void OnStayInAir()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        direction = player.transform.position - transform.position;
    }

    public void ThrowHammer()
    {
        Instantiate(hammer, hammerPos.position, hammerPos.rotation);
    }

    public void OnDashDown()
    {
        rb.useGravity = true;
        isDashing = true;
    }

    public void OnLanded()
    {
        isDashing = false;
        rb.velocity = Vector3.zero;
        SoundFXManager.instance.PlaySoundFXClip(swordSoundClip, transform, 1, false);
    }

    public void SetTarget(GameObject target)
    {
        player = target;
    }
}
