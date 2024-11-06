using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : CultistBaseState
{
    private Transform _enemyTransform;

    private float _fleeDuration = 1.5f; // Duration the cultist should stay in FleeState
    private float _fleeTimer;

    public override void EnterState()
    {
        _fleeTimer = 0f;
        cultist.m_Animator.SetBool("IsRunning", true);
    }

    public override void UpdateState()
    {
        _fleeTimer += Time.deltaTime;


        if (_enemyTransform != null)
        {
            // Determine horizontal direction away from the enemy
            Vector2 fleeDirection = transform.position.x < _enemyTransform.position.x
                ? Vector2.left
                : Vector2.right;

            cultist.RotateCultist(fleeDirection);

            // Move the cultist in the flee direction
            transform.Translate(fleeDirection * cultist.cultistDataSO.fleeSpeed * Time.deltaTime, Space.World);


            if (_fleeTimer >= _fleeDuration)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, _enemyTransform.position);
                if (distanceToEnemy > cultist.cultistDataSO.enemyDetectionRange)
                {
                    cultist.ChangeState(Cultist.ECultistState.Idle);
                }
            }
        }
    }

    public void SetEnemyTransform(Transform enemy)
    {
        _enemyTransform = enemy;
    }

    public override void ExitState()
    {
        cultist.m_Animator.SetBool("IsRunning", false);
    }
}
