using UnityEngine;
using Utils;

public class Enemy : Poolable
{
    [SerializeField] private int m_Health;
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;
    [SerializeField] private State[] m_EnemyStates;
    [SerializeField] private Animator m_Animator;

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

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        print("Updating animation");
        if (m_Animator != null)
        {
            if (m_CurrentState == EEnemyState.Moving) print("Moving");
            m_Animator.SetBool("IsRunning", m_CurrentState == EEnemyState.Moving);
        }
    }


    private void ChangeState(EEnemyState newState)
    {
        m_EnemyStates[(int)m_CurrentState]?.ExitState();
        m_CurrentState = newState;
        m_EnemyStates[(int)m_CurrentState].EnterState();
    }

    public void TakeDamage(int damage)
    {
        if ((m_CurrentHealth -= damage) <= 0)
        {
            // open endpoint to spawn a corpse; either listen to the event in a corpse spawner script, or add it here
            m_EnemyDeathEventSO.value = this;
            FreeToPool();
        }
    }
}