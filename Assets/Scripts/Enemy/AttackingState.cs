using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : State
{
    [SerializeField] private float m_AttackAnimationDuration = 1.0f;

    private IAttackable m_TargetToDamage;
    private float m_TimeElapsed = 0.0f;

    public override void EnterState()
    {
        Attack();
    }

    public override void UpdateState(float deltaTime)
    {
        m_TimeElapsed += deltaTime;
        if (m_TimeElapsed >= m_AttackAnimationDuration)
        {
            IAttackable target = m_Enemy.currentTarget;
            if (target == null)
            {
                m_Enemy.ChangeState(Enemy.EEnemyState.Moving);
                m_Enemy.animator.SetBool("IsAttacking", false);
                m_Enemy.animator.SetBool("IsRunning", true);
            }
            else
            {
                Attack();
            }
        }
    }

    public override void ExitState()
    {
    }

    private void Attack()
    {
        m_TargetToDamage = m_Enemy.currentTarget;
        m_Enemy.animator.SetBool("IsAttacking", true);
        Invoke(nameof(DamageTarget), 0.5f);
        m_TimeElapsed = 0.0f;
    }

    private void DamageTarget()
    {
        m_TargetToDamage.Damage(m_Enemy.enemyDataSO.attackDamage);
    }
}
