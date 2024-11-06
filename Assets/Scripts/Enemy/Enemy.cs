using UnityEngine;
using Utils;

public class Enemy : Poolable
{
    [SerializeField] private int m_Health;
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;
    [SerializeField] private State[] m_EnemyStates;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_DeathAnimationDuration = 1.0f;

    private enum EEnemyState
    {
        Moving = 0,
        Attacking,
        Dying,
        COUNT
    }

    private EEnemyState m_CurrentState = default;
    private int m_CurrentHealth = 0;

    public override void Initialize()
    {
        m_CurrentHealth = m_Health;
        transform.rotation = Quaternion.Euler(0.0f, transform.position.x > 0 ? 180.0f : 0.0f, 0.0f);
        ChangeState(EEnemyState.Moving);
    }   
    private void Awake()
    {
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        m_EnemyStates[(int)m_CurrentState].UpdateState(Time.deltaTime);

        m_Animator.SetBool("IsRunning", m_CurrentState == EEnemyState.Moving);
    }

    private void ChangeState(EEnemyState newState)
    {
        m_EnemyStates[(int)m_CurrentState]?.ExitState();
        m_CurrentState = newState;
        m_EnemyStates[(int)m_CurrentState].EnterState();
    }
    private void OnDeathAnimationComplete()
    {
        m_EnemyDeathEventSO.value = this;
        FreeToPool();
    }
    public void TakeDamage(int damage)
    {
        if ((m_CurrentHealth -= damage) <= 0)
        {
            m_Animator.SetBool("IsDead", true);
            Invoke("OnDeathAnimationComplete", m_DeathAnimationDuration);
        }
    }
}