using UnityEngine;
using Utils;

public class Enemy : Poolable
{
    [SerializeField] private int m_Health;
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;

    private int m_CurrentHealth = 0;

    public override void Initialize()
    {
        m_CurrentHealth = m_Health;
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