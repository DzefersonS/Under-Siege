using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : CultistBaseState
{
    private Vector2 _direction;
    private GameObject _graveyardGO;

    private bool isGoingToGraveyard;


    public override void EnterState()
    {
        _direction = cultist.FindTurningDirection(cultist.deadBody.gameObject);
        cultist.RotateCultist(_direction);
        cultist.m_Animator.SetBool("IsRunning", true);

        isGoingToGraveyard = false;
        LocateGraveyard(cultist);
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

        CheckIfOutOfBaseArea();

        if (!isGoingToGraveyard)
            transform.Translate(_direction * cultist.cultistDataSO.collectSpeed * Time.deltaTime, Space.World);
        else
            transform.Translate(_direction * cultist.cultistDataSO.carrySpeed * Time.deltaTime, Space.World);
    }

    public override void ExitState()
    {
        cultist.m_Animator.SetBool("IsRunning", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DeadBody")
        {
            if (other.GetComponent<DeadBody>().GetClaimant() == cultist)
            {
                cultist.deadBody.transform.parent = transform;
                Vector2 _newDirection = cultist.FindTurningDirection(_graveyardGO);

                if (_newDirection != _direction)
                {
                    _direction = _newDirection;
                    cultist.RotateCultist(_direction);
                }

                isGoingToGraveyard = true;

                cultist.m_Animator.SetBool("IsRunning", false);
                cultist.m_Animator.SetBool("IsIdling", true); //For slow walk 

            }
        }

        if (other.name == "Graveyard")
        {
            cultist.m_Animator.SetBool("IsIdling", true);
            isGoingToGraveyard = false;
            cultist.ChangeState(Cultist.ECultistState.Idle);
        }
    }

    private void LocateGraveyard(Cultist cultist)
    {
        Collider2D[] bodiesInRange = Physics2D.OverlapCircleAll(cultist.transform.position, 100);

        foreach (Collider2D body in bodiesInRange)
        {
            if (body.name == "Graveyard")
            {
                _graveyardGO = body.gameObject;
            }
        }
    }

    private void CheckIfOutOfBaseArea()
    {
        if (cultist.transform.position.x > 25 || cultist.transform.position.x < -25)
        {
            if (cultist.deadBody != null)
                cultist.deadBody.Unclaim();

            cultist.ChangeState(Cultist.ECultistState.Idle);
        }
    }

}