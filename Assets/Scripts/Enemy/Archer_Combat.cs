using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Archer_Combat : CombatController
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ShootArrow());
    }


    private IEnumerator ShootArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - 0.05f);
            HandleShooting();
        }
    }
}
