using UnityEngine;

public class AttackingState : State
{
    [SerializeField] private float m_AttackAnimationDuration = 1.0f;

    private IAttackable m_TargetToDamage;
    private float m_TimeElapsed = 0.0f;

    public override void EnterState()
    {
        m_Enemy.animator.SetBool("IsAttacking", true);
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
            }
            else
            {
                Attack();
            }
        }
    }

    public override void ExitState()
    {
        m_Enemy.animator.SetBool("IsAttacking", false);
    }

    private void Attack()
    {
        m_TargetToDamage = m_Enemy.currentTarget;
        Invoke(nameof(DamageTarget), 0.5f);
        m_TimeElapsed = 0.0f;
    }

    private void DamageTarget()
    {
        m_TargetToDamage.Damage(m_Enemy.enemyDataSO.attackDamage);
    }
}
