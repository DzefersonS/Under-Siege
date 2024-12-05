using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Shrine : MonoBehaviour, IAttackable
{
    [SerializeField] private int m_MaxHealth;
    [SerializeField] private UIManager m_UIManager;
    [SerializeField] private UpgradePurchaseEventSO _upgradePurchaseEventSO;
    [SerializeField] private GameWonEventSO _gameWonEventSO;
    [SerializeField] private Collider2D m_PlayerCollider;

    [SerializeField] private RectTransform m_Healthbar;

    [SerializeField] private AudioSource m_DestroyedSFX;

    private Collider2D m_Collider = default;
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
        m_Collider = GetComponent<Collider2D>();
    }

    private void OnDestroy()
    {
        _upgradePurchaseEventSO.Unregister(ChangeShrineVisual);

    }

    public void CheckIfPlayerInHitbox()
    {
        if (m_Collider.bounds.Intersects(m_PlayerCollider.bounds))
        {
            m_UIManager.EnableShrineCanvas();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_UIManager.EnableShrineCanvas();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            m_UIManager.DisableShrineCanvas();
    }

    public void Damage(int damageAmount)
    {
        m_CurrentHealth -= damageAmount;
        m_HealthbarSizeDelta.x = m_CurrentHealth * m_HealthbarWidthFactor;
        m_Healthbar.sizeDelta = m_HealthbarSizeDelta;

        if (m_CurrentHealth <= 0)
        {
            ChangeShrineVisualToDestroyed();
            m_DestroyedSFX.Play();

            _gameWonEventSO.value = false; //Loss
        }
    }

    private void ChangeShrineVisual()
    {
        if (_upgradePurchaseEventSO.value == 5)
        {
            var transform = gameObject.transform;
            int nextIndex = m_ShrineUpgradeIndex + 1;

            m_CurrentHealth = m_MaxHealth;
            m_HealthbarSizeDelta.x = m_CurrentHealth * m_HealthbarWidthFactor;
            m_Healthbar.sizeDelta = m_HealthbarSizeDelta;

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

    private void ChangeShrineVisualToDestroyed()
    {
        // Deactivate current shrine
        transform.GetChild(m_ShrineUpgradeIndex).gameObject.SetActive(false);
        // Activate the destroyed visual
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
