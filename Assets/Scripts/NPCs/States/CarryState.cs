using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryState : CultistBaseState
{
    private GameObject _graveyardGO;
    private Vector2 _direction;
    private DeadBody _deadbody;
    public override void EnterState()
    {
        cultist.isFree = false; // Make sure cultist is not free
        cultist.isCarryingBody = true;

        LocateGraveyard(cultist);
        _direction = cultist.FindTurningDirection(_graveyardGO);

        cultist.RotateCultist(_direction);

    }

    public void SetDeadBody(DeadBody deadbody)
    {
        _deadbody = deadbody;
    }

    public override void UpdateState()
    {
        if (cultist.CheckForEnemies())
        {
            _deadbody.Unclaim();
        }

        //Go to graveyard
        if ((Mathf.Abs(transform.position.x - _graveyardGO.transform.position.x) > 0.01f))
        {
            transform.Translate(_direction * cultist.cultistDataSO.carrySpeed * Time.deltaTime, Space.World);
        }
        else
        {
            //_deadbody.FreeToPool();
            cultist.ChangeState(Cultist.ECultistState.Idle);
        }
    }

    public override void ExitState()
    {

    }



    private void LocateGraveyard(Cultist cultist)
    {
        Collider2D[] bodiesInRange = Physics2D.OverlapCircleAll(cultist.transform.position, 9999);

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