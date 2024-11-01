using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : CultistBaseState
{
    private Transform enemyTransform;
    private float fleeSpeed = 3f;

    private float fleeDuration = 1.5f; // Duration the cultist should stay in FleeState
    private float fleeTimer;

    public FleeState(Transform enemy)
    {
        enemyTransform = enemy;
    }

    public override void EnterState(Cultist cultist)
    {
        Debug.Log("State: Flee");
        fleeTimer = 0f;

        cultist.isFree = false;
        if (cultist.isCarryingBody)
        {
            cultist.transform.GetChild(0).gameObject.SetActive(false);//Enable the illusion of dead body
            cultist.isCarryingBody = false;


            //somehow instantiate a body in here

        }
    }

    public override void UpdateState(Cultist cultist)
    {
        fleeTimer += Time.deltaTime;


        if (enemyTransform != null)
        {
            // Determine horizontal direction away from the enemy
            Vector2 fleeDirection = cultist.transform.position.x < enemyTransform.position.x
                ? Vector2.left
                : Vector2.right;

            cultist.RotateCultist(fleeDirection);

            // Move the cultist in the flee direction
            cultist.transform.Translate(fleeDirection * fleeSpeed * Time.deltaTime, Space.World);


            if (fleeTimer >= fleeDuration)
            {
                float distanceToEnemy = Vector2.Distance(cultist.transform.position, enemyTransform.position);
                if (distanceToEnemy > 2f)
                {
                    cultist.ChangeState(cultist.IdleState);
                }
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Flee State");
    }

}
