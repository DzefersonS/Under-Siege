using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : MonoBehaviour
{
    [SerializeField] public Animator m_Animator;
    [SerializeField] public CultistDataSO cultistDataSO;
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] private CultistBaseState[] m_CultistStates;

    public ECultistState m_CurrentState = default;

    public enum ECultistState
    {
        Idle = 0,
        Collect,
        Carry,
        Flee,
        COUNT
    }

    private void Start()
    {
        foreach (var state in m_CultistStates)
        {
            state.SetCultist(this);
        }

        m_CurrentState = ECultistState.Idle;
        ChangeState(ECultistState.Idle);
    }
    private void Update()
    {
        m_CultistStates[(int)m_CurrentState].UpdateState();
    }

    public void ChangeState(ECultistState newState, DeadBody deadbody = null, Transform enemyTransform = null)
    {
        m_CultistStates[(int)m_CurrentState]?.ExitState();
        m_CurrentState = newState;

        if (newState == ECultistState.Collect && deadbody != null)
        {
            ((CollectState)m_CultistStates[(int)ECultistState.Collect]).SetDeadBody(deadbody);
        }
        if (newState == ECultistState.Flee && enemyTransform != null)
        {
            ((FleeState)m_CultistStates[(int)ECultistState.Flee]).SetEnemyTransform(enemyTransform);
        }
        if (newState == ECultistState.Carry && deadbody != null)
        {
            ((CarryState)m_CultistStates[(int)ECultistState.Carry]).SetDeadBody(deadbody);
        }

        m_CultistStates[(int)m_CurrentState].EnterState();
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

    public bool CheckForEnemies()
    {
        // 2m detection range
        // Check if an enemy is nearby
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, cultistDataSO.enemyDetectionRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                ChangeState(ECultistState.Flee, null, hitCollider.transform);
                return true;
            }
        }
        return false;
    }

    public void FreeDeadBody(DeadBody deadBody)
    {
        deadBody.FreeToPool();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, cultistDataSO.enemyDetectionRange);
    }
}