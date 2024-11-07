using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public abstract class State : MonoBehaviour
{
    protected Enemy m_Enemy = default;

    private void Awake()
    {
        m_Enemy = GetComponent<Enemy>();
        enabled = false;
    }

    public abstract void EnterState();
    public abstract void UpdateState(float deltaTime);
    public abstract void ExitState();
}
