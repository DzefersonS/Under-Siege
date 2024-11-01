using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : MonoBehaviour
{
    CultistBaseState currentState = default;
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    public IdleState IdleState = new IdleState();

    public bool isFree { get; set; }
    public bool isCarryingBody { get; set; }


    public string stateName; // debugging, remove later

    private float _detectionRange = 2f;

    private void Start()
    {
        currentState = IdleState;
        isFree = true;
        isCarryingBody = false;
        currentState.EnterState(this);
    }
    private void Update()
    {
        stateName = currentState.ToString();
        currentState.UpdateState(this);
    }

    public void ChangeState(CultistBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState(this);
    }

    public void RotateCultist(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }
    }

    //finds out if an object needs to turn left or right to face the target gameObject
    public Vector2 FindTurningDirection(GameObject gameObject)
    {
        return (gameObject.transform.position.x < this.transform.position.x) ? Vector2.left : Vector2.right;
    }
    public void CheckForEnemies()
    {
        // 2m detection range
        // Check if an enemy is nearby
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _detectionRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                ChangeState(new FleeState(hitCollider.transform));
                return;
            }
        }

    }


    public void FreeDeadBody(DeadBody deadBody)
    {
        deadBody.FreeToPool();
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

}

