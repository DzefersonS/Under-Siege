using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour, IAttackable
{
    [SerializeField] private int m_MaxHealth;
    [SerializeField] private GameObjectEventSO _gameObjectEventSO;
    [SerializeField] private UpgradePurchaseEventSO _upgradePurchaseEventSO;
    [SerializeField] private GameWonEventSO _gameWonEventSO;


    [SerializeField] private RectTransform m_Healthbar;

    private Vector2 m_HealthbarSizeDelta = default;
    private float m_HealthbarWidth = default;
    private float m_HealthbarWidthFactor = default;
    private int m_CurrentHealth = default;


    private int m_ShrineUpgradeIndex;

    private void Awake()
    {
        _upgradePurchaseEventSO.Register(ChangeShrineVisual);

        m_HealthbarWidth = m_Healthbar.sizeDelta.x;
        m_HealthbarSizeDelta = m_Healthbar.sizeDelta;
        m_CurrentHealth = m_MaxHealth;
        m_HealthbarWidthFactor = m_HealthbarWidth / m_MaxHealth;
    }

    private void OnDestroy()
    {
        _upgradePurchaseEventSO.Unregister(ChangeShrineVisual);

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

        if (m_CurrentHealth <= 0)
            _gameWonEventSO.value = false; //Loss
    }

    private void ChangeShrineVisual()
    {
        if (_upgradePurchaseEventSO.value == 5)
        {
            var transform = gameObject.transform;
            int nextIndex = m_ShrineUpgradeIndex + 1;

            // Check if the next index is within bounds
            if (nextIndex < transform.childCount)
            {
                // Deactivate current shrine
                transform.GetChild(m_ShrineUpgradeIndex).gameObject.SetActive(false);

                // Activate the next shrine
                m_ShrineUpgradeIndex++;
                transform.GetChild(m_ShrineUpgradeIndex).gameObject.SetActive(true);
            }
        }
    }


}
