using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : CultistBaseState
{
    private float _deathAnimationDuration = 0.5f;
    private float _deathAnimationTimer;

    public override void EnterState()
    {
        cultist.m_Animator.SetBool("IsDead", true);
        _deathAnimationTimer = 0;
        if (cultist.deadBody != null)
            cultist.deadBody.Unclaim();
        //maybe create dead body here
    }

    public override void UpdateState()
    {
        _deathAnimationTimer += Time.deltaTime;

        if (_deathAnimationTimer >= _deathAnimationDuration)
            ExitState();

        //after x amount of seconds go to exit state
    }


    public override void ExitState()
    {
        //Create another dead body of cultist here
        //Free to Pool( once its poolable)
        Destroy(gameObject);
    }

}
