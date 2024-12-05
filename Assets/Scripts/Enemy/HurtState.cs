using UnityEngine;

public class HurtState : State
{
    private Animator m_Animator = default;

    public override void EnterState()
    {
    }

    public override void UpdateState(float deltaTime)
    {
        AnimatorStateInfo stateInfo = m_Enemy.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Hurt") && stateInfo.normalizedTime >= 1.0f)
        {
            m_Enemy.animator.SetBool("IsHurt", false);
            m_Enemy.ChangeState(m_Enemy.previousState);
        }
    }

    public override void ExitState()
    {
    }
}