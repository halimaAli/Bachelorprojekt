using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackState : BossBaseState
{
    private int spins = 1;
    private float distance;
    public SpinAttackState(BossStateController context) : base(context)
    {
    }

    public override void Enter()
    {
        FacePlayer();
        context.animator.SetTrigger("SpinAttack");
    }

    private void FacePlayer()
    {
        context.FacePlayer();
    }
}
