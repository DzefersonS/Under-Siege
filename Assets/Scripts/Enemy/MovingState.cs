using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{

    public override void EnterState()
    {
    }

    public override void UpdateState(float deltaTime)
    {
        transform.position += m_Enemy.movementVector * deltaTime;
    }

    public override void ExitState()
    {
    }
}