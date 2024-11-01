using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CollectState : CultistBaseState
{
    private DeadBody _deadbody;
    Vector2 direction;
    private float speed = 3f;

    public CollectState(DeadBody deadbody)
    {
        _deadbody = deadbody;
    }

    //On entered state, remove yourself from available cultists,
    //rotate to the desired direction,
    public override void EnterState(Cultist cultist)
    {
        Debug.Log("State: Collect");
        cultist.isFree = false;
        cultist.isCarryingBody = false;

        direction = cultist.FindTurningDirection(_deadbody.gameObject);
        cultist.RotateCultist(direction);

    }
    public override void UpdateState(Cultist cultist)
    {
        cultist.CheckForEnemies();


        //Go to dead body
        if ((Mathf.Abs(cultist.transform.position.x - _deadbody.transform.position.x) > 0.01f))
        {
            cultist.transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else
        {
            cultist.transform.GetChild(0).gameObject.SetActive(true);//Enable the illusion of dead body
            cultist.FreeDeadBody(_deadbody);
            cultist.ChangeState(new CarryState());
        }

    }

    public override void ExitState()
    {


    }

}



