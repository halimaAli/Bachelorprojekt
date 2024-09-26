using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    private PlayerMovement movement;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [Header("Particle FX")]
    [SerializeField] private GameObject jumpFX;
    [SerializeField] private GameObject landFX;
    private ParticleSystem jumpParticle;
    private ParticleSystem landParticle;


    [SerializeField] AudioSource walkingSound;
    [SerializeField] AudioSource slidingSound;

    public bool Jumping { private get; set; }
    public bool Landed { private get; set; }
    public bool Walking { private get; set; }
    public bool Sliding { private get; set; }

    public float currentVelY;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = _spriteRenderer.GetComponent<Animator>();

    }

    private void LateUpdate()
    {
        CheckAnimationState();
    }

    private void CheckAnimationState()
    {
        if (Jumping)
        {
            _animator.SetBool("isJumping", true);
            /*GameObject obj = Instantiate(jumpFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);*/
            Jumping = false;
            return;
        }

        if (Landed)
        {
           /* GameObject obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);*/
            Landed = false;
            return;
        }

        if (!walkingSound.isPlaying && Walking)
        {
            walkingSound.Play();
        }

        if (walkingSound.isPlaying && !Walking)
        {
            walkingSound.Stop();
        }


        /*if (!slidingSound.isPlaying && Sliding)
        {
            slidingSound.Play();
        }

        if (slidingSound.isPlaying && !Sliding)
        {
            slidingSound.Stop();
        }*/
        //_animator.SetFloat("Vel Y", mov.RB.velocity.y);
    }
}
