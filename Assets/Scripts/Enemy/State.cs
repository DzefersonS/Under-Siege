using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    private void Awake()
    {
        enabled = false;
    }

    public abstract void EnterState();
    public abstract void UpdateState(float deltaTime);
    public abstract void ExitState();
}
