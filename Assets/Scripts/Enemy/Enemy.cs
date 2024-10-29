using UnityEngine;
using Utils;

public class Enemy : Poolable
{
    [SerializeField] private int m_Health;
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;

    public void TakeDamage(int damage)
    {
        if ((m_Health -= damage) < 0)
        {
            m_EnemyDeathEventSO.value = this;
            FreeToPool();
        }
    }
}