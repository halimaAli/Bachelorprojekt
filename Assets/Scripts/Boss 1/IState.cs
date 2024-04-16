using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    void Enter();
    void UpdateState();
    void FixedUpdate();
    void Exit();
}
