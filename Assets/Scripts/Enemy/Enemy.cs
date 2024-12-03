using UnityEngine;
using Utils;

public class Enemy : Poolable, IAttackable
{
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;
    [SerializeField] private TransformEventSO m_BodySpawnPositionSO;
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
        Hurt,
        Dying,
        COUNT
    }

    private EEnemyState m_PreviousState = default;
    private EEnemyState m_CurrentState = default;
    private IAttackable m_CurrentTarget = default;
    private Vector3 m_MovementVector = default;
    private float m_CurrentHealth = 0;

    public Animator animator => m_Animator;
    public EnemyDataSO enemyDataSO => m_EnemyDataSO;
    public IAttackable currentTarget => m_CurrentTarget;
    public Vector3 movementVector => m_MovementVector;
    public EEnemyState previousState => m_PreviousState;

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

        //Sort by distance, otherwise order of elements is undocumented behaviour
        System.Array.Sort(hitColliders, (a, b) =>
        {
            float distanceLHS = Vector2.Distance(transform.position, a.transform.position);
            float distanceRHS = Vector2.Distance(transform.position, b.transform.position);
            return distanceLHS.CompareTo(distanceRHS);
        });

        IAttackable newTarget = null;

        if (!string.IsNullOrEmpty(m_EnemyDataSO.priorityTargetTag))
        {
            foreach (Collider2D col in hitColliders)
            {
                IAttackable attackable = col.GetComponent<IAttackable>();
                if (attackable != null && !col.CompareTag("Enemy") && col.CompareTag(m_EnemyDataSO.priorityTargetTag))
                {
                    newTarget = attackable;
                    break;
                }
            }
        }

        if (newTarget == null)
        {
            foreach (Collider2D col in hitColliders)
            {
                IAttackable attackable = col.GetComponent<IAttackable>();
                if (attackable != null && !col.CompareTag("Enemy"))
                {
                    newTarget = attackable;
                    break;
                }
            }
        }

        if (newTarget != null && m_CurrentState == EEnemyState.Moving)
        {
            m_CurrentTarget = newTarget;
            ChangeState(EEnemyState.Attacking);
        }
        else
        {
            m_CurrentTarget = newTarget;
        }
    }

    private void OnDeathAnimationComplete()
    {
        m_EnemyDeathEventSO.value = this;
        m_BodySpawnPositionSO.value = transform.GetChild(0);
        FreeToPool();
    }

    public void Damage(int damageAmount)
    {
        m_HitSFX.Play();

        if ((m_CurrentHealth -= damageAmount) <= 0)
        {
            m_Animator.SetBool("IsDead", true);
            Invoke(nameof(OnDeathAnimationComplete), m_DeathAnimationDuration);
        }
        else
        {
            m_Animator.CrossFade("Hurt", 0.0f);
            m_Animator.SetBool("IsHurt", true);
            m_PreviousState = m_CurrentState;
            ChangeState(EEnemyState.Hurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_EnemyDataSO.attackRange);
    }
}