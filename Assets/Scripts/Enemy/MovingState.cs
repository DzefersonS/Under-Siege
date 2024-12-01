using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{

    public override void EnterState()
    {
        m_Enemy.animator.SetBool("IsRunning", true);
        m_Enemy.animator.CrossFade("Run", 0);
    }

    public override void UpdateState(float deltaTime)
    {
        transform.position += m_Enemy.movementVector * deltaTime;
    }

    public override void ExitState()
    {
        m_Enemy.animator.SetBool("IsRunning", false);
    }
}