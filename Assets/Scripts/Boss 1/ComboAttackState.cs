using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackState : BossBaseState
{
    public ComboAttackState(BossStateController context) : base(context)
    {
    }

    public override void Enter()
    {
        context.animator.SetTrigger("Combo Attack");
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit() {
        base.Exit();
    }



}
