using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class IdleState : CultistBaseState
{
    private bool _isMoving = false;
    private int _moveXCoord;
    private Vector2 _direction;

    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
        if (!_isMoving)
        {
            SetNewTargetPosition();
        }
        Move(_moveXCoord, _direction);

        cultist.CheckForEnemies();
    }

    public override void ExitState()
    {
        _isMoving = false;
    }

    private void SetNewTargetPosition()
    {
        _moveXCoord = Random.Range(-10, 11);

        _direction = cultist.transform.position.x < _moveXCoord
            ? Vector2.right
            : Vector2.left;

        cultist.RotateCultist(_direction);

        _isMoving = true;
    }

    private void Move(float xCoord, Vector2 direction)
    {

        if (Mathf.Abs(cultist.transform.position.x - xCoord) > 0.01f)
        {
            cultist.transform.Translate(direction * cultist.cultistDataSO.idleSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            _isMoving = false;
        }
    }
}



