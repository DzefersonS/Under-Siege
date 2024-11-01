using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryState : CultistBaseState
{
    private GameObject _graveyardGO;
    private Vector2 _direction;
    private float speed = 2.5f;
    public override void EnterState()
    {
        Debug.Log("State: Carry");
        cultist.isFree = false; // Make sure cultist is not free
        cultist.isCarryingBody = true;

        LocateGraveyard(cultist);
        _direction = cultist.FindTurningDirection(_graveyardGO);

        cultist.RotateCultist(_direction);

    }

    public override void UpdateState()
    {
        cultist.CheckForEnemies();

        //Go to graveyard
        if ((Mathf.Abs(cultist.transform.position.x - _graveyardGO.transform.position.x) > 0.01f))
        {
            cultist.transform.Translate(_direction * speed * Time.deltaTime, Space.World);
        }
        else
        {
            cultist.transform.GetChild(0).gameObject.SetActive(false);//Enable the illusion of dead body
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
