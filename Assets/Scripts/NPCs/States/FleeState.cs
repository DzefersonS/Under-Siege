using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : CultistBaseState
{
    private Transform _enemyTransform;
    private float fleeDuration = 1.5f; // Duration the cultist should stay in FleeState
    private float fleeTimer;

    public void SetEnemyTransform(Transform enemy)
    {
        _enemyTransform = enemy;
    }


    public override void EnterState()
    {
        fleeTimer = 0f;

        cultist.isFree = false;
        if (cultist.isCarryingBody)
        {
            cultist.transform.GetChild(0).gameObject.SetActive(false);//Disable the illusion of dead body
            cultist.isCarryingBody = false;

            //somehow instantiate a body in here
        }
    }

    public override void UpdateState()
    {
        fleeTimer += Time.deltaTime;


        if (_enemyTransform != null)
        {
            // Determine horizontal direction away from the enemy
            Vector2 fleeDirection = cultist.transform.position.x < _enemyTransform.position.x
                ? Vector2.left
                : Vector2.right;

            cultist.RotateCultist(fleeDirection);

            // Move the cultist in the flee direction
            cultist.transform.Translate(fleeDirection * cultist.cultistDataSO.fleeSpeed * Time.deltaTime, Space.World);


            if (fleeTimer >= fleeDuration)
            {
                float distanceToEnemy = Vector2.Distance(cultist.transform.position, _enemyTransform.position);
                if (distanceToEnemy > 2f)
                {
                    cultist.ChangeState(Cultist.ECultistState.Idle);
                }
            }
        }
    }
    public override void ExitState()
    {
        Debug.Log("Exiting Flee State");
    }



}
