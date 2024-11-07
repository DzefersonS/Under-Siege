using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour, IAttackable
{
    [SerializeField] private int m_MaxHealth;
    [SerializeField] private GameObjectEventSO _gameObjectEventSO;
    [SerializeField] private RectTransform m_Healthbar;

    private Vector2 m_HealthbarSizeDelta = default;
    private float m_HealthbarWidth = default;
    private float m_HealthbarWidthFactor = default;
    private int m_CurrentHealth = default;

    private void Awake()
    {
        m_HealthbarWidth = m_Healthbar.sizeDelta.x;
        m_HealthbarSizeDelta = m_Healthbar.sizeDelta;
        m_CurrentHealth = m_MaxHealth;
        m_HealthbarWidthFactor = m_HealthbarWidth / m_MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _gameObjectEventSO.value = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            _gameObjectEventSO.value = this.gameObject;

    }

    public void Damage(int damageAmount)
    {
        m_CurrentHealth -= damageAmount;
        m_HealthbarSizeDelta.x = m_CurrentHealth * m_HealthbarWidthFactor;
        m_Healthbar.sizeDelta = m_HealthbarSizeDelta;
    }
}
