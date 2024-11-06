using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryState : CultistBaseState
{
    private GameObject _graveyardGO;
    private Vector2 _direction;

    public override void EnterState()
    {
        LocateGraveyard(cultist);
        _direction = cultist.FindTurningDirection(_graveyardGO);
        cultist.RotateCultist(_direction);
        cultist.m_Animator.SetBool("IsIdling", true);
    }

    public override void UpdateState()
    {
        if (cultist.CheckForEnemies())
        {
            cultist.deadBody.Unclaim();
            return;
        }

        //Go to graveyard
        if ((Mathf.Abs(transform.position.x - _graveyardGO.transform.position.x) > 0.5f))
        {
            transform.Translate(_direction * cultist.cultistDataSO.carrySpeed * Time.deltaTime, Space.World);
        }
        else
        {
            cultist.ChangeState(Cultist.ECultistState.Idle);
        }
    }

    public override void ExitState()
    {
        cultist.m_Animator.SetBool("IsIdling", false);

        if (cultist.deadBody != null)
            cultist.deadBody.Unclaim();
    }

    private void LocateGraveyard(Cultist cultist)
    {
        Collider2D[] bodiesInRange = Physics2D.OverlapCircleAll(cultist.transform.position, 100);

        foreach (Collider2D body in bodiesInRange)
        {
            // Check if the collider is tagged "Graveyard"
            if (body.name == "Graveyard")
            {
                _graveyardGO = body.gameObject;
            }
        }
    }

}
