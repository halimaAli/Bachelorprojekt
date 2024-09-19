using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackState : MonoBehaviour
{
    private Animator animator;
    private bool isSpinning;
    public int speed;
    [SerializeField] AudioClip spinSoundClip;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        { 
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    public void StartSpinAttack()
    {
        SoundFXManager.instance.PlaySoundFXClip(spinSoundClip, transform, 1, false);
        isSpinning = true;
        
        if (transform.localScale.x > 0)
        {
            speed = 10;
        }
        else
        {
            speed = -10;
        }
    }

    public void StopSpinAttack()
    {
        isSpinning = false;
    }

}
