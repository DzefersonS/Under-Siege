using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{
    [SerializeField] private float m_MovementSpeed;

    private Vector3 m_MovementVector;

    public override void EnterState()
    {
        m_MovementVector = new Vector3()
        {
            x = m_MovementSpeed * -Mathf.Sign(transform.position.x),
            y = 0.0f
        };
    }

    public override void UpdateState(float deltaTime)
    {
        transform.position += m_MovementVector * deltaTime;
    }

    public override void ExitState()
    {
    }
}