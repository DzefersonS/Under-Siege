using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Cultist : Poolable, IAttackable
{
    [SerializeField] public Animator m_Animator;
    [SerializeField] public CultistDataSO cultistDataSO;
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] public CultistEventSO _cultistDeathEventSO;


    [SerializeField] private CultistBaseState[] m_CultistStates;

    public DeadBody deadBody;
    public int _health;
    private bool _hasDied = false;



    public ECultistState m_CurrentState = default;

    public enum ECultistState
    {
        Idle = 0,
        Collect,
        Flee,
        Death,
        COUNT
    }

    private void Start()
    {
        foreach (var state in m_CultistStates)
        {
            state.SetCultist(this);
        }

        _health = cultistDataSO.health;
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

        if (m_CurrentState == ECultistState.Death && newState != ECultistState.Death)
            return; // Once in Death state, no other state should be entered

        if (newState == ECultistState.Collect && deadbody != null)
        {
            deadBody = deadbody;
        }
        if (newState == ECultistState.Flee && enemyTransform != null)
        {
            ((FleeState)m_CultistStates[(int)ECultistState.Flee]).SetEnemyTransform(enemyTransform);
        }

        m_CultistStates[(int)m_CurrentState].EnterState();
    }

    public void RotateCultist(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }
    }

    //finds out if an object needs to turn left or right to face the target gameObject
    public Vector2 FindTurningDirection(GameObject gameObject)
    {
        return (gameObject.transform.position.x < this.transform.position.x) ? Vector2.left : Vector2.right;
    }

    public bool CheckForEnemies()
    {
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

    public void Damage(int damageAmount)
    {
        if (m_CurrentState == ECultistState.Death)
            return; // Prevent redundant state changes to Death

        _health -= damageAmount;

        if (_health <= 0)
            ChangeState(ECultistState.Death);
    }

    public void OnDeathAnimationComplete()
    {
        if (_hasDied) return; // Prevent multiple executions
        _hasDied = true;

        _cultistDeathEventSO.value = this;
    }
}