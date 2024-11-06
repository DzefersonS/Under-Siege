using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : CultistBaseState
{
    private Vector2 _direction;

    public override void EnterState()
    {
        _direction = cultist.FindTurningDirection(cultist.deadBody.gameObject);
        cultist.RotateCultist(_direction);
        cultist.m_Animator.SetBool("IsRunning", true);
    }

    public override void UpdateState()
    {
        if (cultist.CheckForEnemies())
        {
            cultist.deadBody.Unclaim();
            return;
        }


        //Go to dead body
        if ((Mathf.Abs(cultist.transform.position.x - cultist.deadBody.transform.position.x) > 0.1f))
        {
            transform.Translate(_direction * cultist.cultistDataSO.collectSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            cultist.deadBody.transform.parent = transform;
            cultist.ChangeState(Cultist.ECultistState.Carry, cultist.deadBody);
        }

    }

    public override void ExitState()
    {
        cultist.m_Animator.SetBool("IsRunning", false);
    }
}