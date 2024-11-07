using UnityEngine;
using Utils;

public class Enemy : Poolable, IAttackable
{
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;
    [SerializeField] private State[] m_EnemyStates;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private EnemyDataSO m_EnemyDataSO;
    [SerializeField] private AudioSource m_HitSFX;
    [SerializeField] private float m_DeathAnimationDuration = 1.0f;
    [SerializeField] private float m_MovementSpeed;

    public enum EEnemyState
    {
        Moving = 0,
        Attacking,
        Dying,
        COUNT
    }

    private EEnemyState m_CurrentState = default;
    private IAttackable m_CurrentTarget;
    private Vector3 m_MovementVector;
    private float m_CurrentHealth = 0;

    public Animator animator => m_Animator;
    public EnemyDataSO enemyDataSO => m_EnemyDataSO;
    public IAttackable currentTarget => m_CurrentTarget;
    public Vector3 movementVector => m_MovementVector;
    public EEnemyState currentState => m_CurrentState;

    public override void Initialize()
    {
        m_CurrentHealth = m_EnemyDataSO.maxHealth;
        transform.rotation = Quaternion.Euler(0.0f, transform.position.x > 0 ? 180.0f : 0.0f, 0.0f);

        m_MovementVector = new Vector3()
        {
            x = m_MovementSpeed * -Mathf.Sign(transform.position.x),
            y = 0.0f
        };

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
        m_EnemyStates[(int)m_CurrentState]?.UpdateState(Time.deltaTime);

        m_Animator.SetBool("IsRunning", m_CurrentState == EEnemyState.Moving);

        LookForAttackableTargets();
    }

    public void ChangeState(EEnemyState newState)
    {
        m_EnemyStates[(int)m_CurrentState]?.ExitState();
        m_CurrentState = newState;
        m_EnemyStates[(int)m_CurrentState].EnterState();
    }

    public void LookForAttackableTargets()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, m_EnemyDataSO.attackRange);

        IAttackable newTarget = null;

        foreach (Collider2D col in hitColliders)
        {
            IAttackable attackable = col.GetComponent<IAttackable>();
            if (attackable != null && !col.CompareTag("Enemy"))
            {
                newTarget = attackable;
                if (m_CurrentState == EEnemyState.Moving)
                {
                    m_CurrentTarget = newTarget;
                    ChangeState(EEnemyState.Attacking);
                }
            }
        }

        m_CurrentTarget = newTarget;
    }

    private void OnDeathAnimationComplete()
    {
        m_EnemyDeathEventSO.value = this;
        FreeToPool();
    }

    public void Damage(int damageAmount)
    {
        m_HitSFX.Play();

        if ((m_CurrentHealth -= damageAmount) <= 0)
        {
            m_CurrentState = EEnemyState.Dying;
            m_Animator.SetBool("IsDead", true);
            Invoke("OnDeathAnimationComplete", m_DeathAnimationDuration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_EnemyDataSO.attackRange);
    }
}