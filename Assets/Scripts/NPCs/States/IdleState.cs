using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class IdleState : CultistBaseState
{
    private bool isMoving = false;
    private int moveXCoord;
    Vector2 direction;

    public override void EnterState()
    {
        cultist.isFree = true;
        cultist.isCarryingBody = false;
    }
    public override void UpdateState()
    {
        if (!isMoving)
        {
            SetNewTargetPosition();
        }
        Move(moveXCoord, direction);

        cultist.CheckForEnemies();
    }

    public override void ExitState()
    {
        isMoving = false;
    }

    private void SetNewTargetPosition()
    {
        moveXCoord = Random.Range(-10, 11);

        direction = cultist.transform.position.x < moveXCoord
            ? Vector2.right
            : Vector2.left;

        cultist.RotateCultist(direction);

        isMoving = true;
    }

    private void Move(float xCoord, Vector2 direction)
    {

        if (Mathf.Abs(cultist.transform.position.x - xCoord) > 0.01f)
        {
            cultist.transform.Translate(direction * cultist.cultistDataSO.idleSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            isMoving = false;
        }
    }
}



