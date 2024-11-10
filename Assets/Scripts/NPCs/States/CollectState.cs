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
            if (cultist.deadBody != null)
            {
                cultist.deadBody.Unclaim();
                cultist.deadBody = null;
            }
            return;
        }

        transform.Translate(_direction * cultist.cultistDataSO.collectSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DeadBody")
        {
            if (other.GetComponent<DeadBody>().GetClaimant() == cultist)
            {
                cultist.deadBody.transform.parent = transform;
                cultist.ChangeState(Cultist.ECultistState.Carry, cultist.deadBody);
            }
        }
    }


    public override void ExitState()
    {
        cultist.m_Animator.SetBool("IsRunning", false);
    }
}