using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : CultistBaseState
{
    private float _deathAnimationDuration = 0.75f;
    private float _deathAnimationTimer;


    public override void EnterState()
    {
        cultist.m_Animator.SetBool("IsDead", true);
        _deathAnimationTimer = 0;

        if (cultist.deadBody != null)
            cultist.deadBody.Unclaim();
    }

    public override void UpdateState()
    {
        _deathAnimationTimer += Time.deltaTime;

        if (_deathAnimationTimer >= _deathAnimationDuration)
            ExitState();
    }

    public override void ExitState()
    {
        cultist.OnDeathAnimationComplete();
    }

}
