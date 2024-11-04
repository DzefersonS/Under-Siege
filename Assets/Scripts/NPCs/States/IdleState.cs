using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class IdleState : CultistBaseState
{
    private float detectionRange = 2f;
    private float idleMoveSpeed = 0.5f;
    private float idleMovementDirection = 1f;
    public override void EnterState()
    {
        Debug.Log("State: Idle");
        cultist.isFree = true;
        cultist.isCarryingBody = false;

    }
    public override void UpdateState()
    {
        cultist.transform.Translate(Vector2.zero * cultist.cultistDataSO.idleSpeed * Time.deltaTime);

        cultist.CheckForEnemies();

    }

    public override void ExitState()
    {
        idleMovementDirection = 0f;
    }




}
