using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : CultistBaseState
{
    public override void EnterState()
    {
        cultist.m_Animator.SetBool("IsDead", true);
        cultist.deadBody.Unclaim();
        //maybe create dead body here
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

}
