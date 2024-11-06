using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : CultistBaseState
{
    private DeadBody _deadbody;
    Vector2 direction;

    public void SetDeadBody(DeadBody deadbody)
    {
        _deadbody = deadbody;
    }

    //On entered state, remove yourself from available cultists,
    //rotate to the desired direction,`
    public override void EnterState()
    {
        cultist.isFree = false;
        cultist.isCarryingBody = false;

        direction = cultist.FindTurningDirection(_deadbody.gameObject);
        cultist.RotateCultist(direction);

    }
    public override void UpdateState()
    {
        if (cultist.CheckForEnemies())
        {
            _deadbody.Unclaim();
        }


        //Go to dead body
        if ((Mathf.Abs(cultist.transform.position.x - _deadbody.transform.position.x) > 0.5f))
        {
            transform.Translate(direction * cultist.cultistDataSO.collectSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            _deadbody.transform.parent = transform;
            cultist.ChangeState(Cultist.ECultistState.Carry, _deadbody);
        }

    }

    public override void ExitState()
    {


    }

}



