using UnityEngine;

public class FireballPowerup : PowerupBase
{
    [SerializeField] private GameObject m_FireballPrefab;
    [SerializeField] private Transform m_FirePoint;
    [SerializeField] private GameObject m_AoeIndicator;
    [SerializeField] private float m_ReducedCooldown = 2.5f;
    [SerializeField] private int m_BaseDamage = 30;
    [SerializeField] private int m_IncreasedDamage = 50;
    [SerializeField] private float m_BaseRadius = 1.77f;
    [SerializeField] private float m_IncreasedRadius = 2.4f;
    
    private Vector3 targetPosition = default;
    private float m_CurrentlyUsedRadius = 0.0f;
    private int m_CurrentlyUsedDamage = 0;
    private bool isAiming = false;

    protected override void Start()
    {
        base.Start();
        m_AoeIndicator?.SetActive(false);
        m_CurrentlyUsedRadius = m_BaseRadius;
        m_CurrentlyUsedDamage = m_BaseDamage;
    }

    protected override void Update()
    {
        base.Update();

        if (isAiming)
        {
            UpdateAOEIndicator();
        }
    }

    protected override void ActivatePowerup()
    {
        if (!isAiming)
        {
            StartAiming();
        }
        else
        {
            Fire();
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        m_AoeIndicator?.SetActive(true);
    }

    private void Fire()
    {
        isAiming = false;
        m_AoeIndicator?.SetActive(false);

        targetPosition = m_AoeIndicator.transform.position;

        // Instantiate and fire the fireball
        GameObject fireball = Instantiate(m_FireballPrefab, m_FirePoint.position, Quaternion.identity);
        Fireball fireballScript = fireball.GetComponent<Fireball>();
        if (fireballScript != null)
        {
            fireballScript.Activate(targetPosition, m_CurrentlyUsedRadius, m_CurrentlyUsedDamage);
        }

        StartCooldown();
    }

    private void UpdateAOEIndicator()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0.0f;
        if (m_AoeIndicator != null)
        {
            m_AoeIndicator.transform.position = new Vector3(cursorPosition.x, m_AoeIndicator.transform.position.y, 0.0f);
        }
        targetPosition = cursorPosition;
    }

    protected override void HandleUpgrade(int level)
    {
        switch (level)
        {
            case 2:
                {
                    currentlyUsedCooldownDuration = m_ReducedCooldown;
                    if(cooldownTimer > m_ReducedCooldown) cooldownTimer = m_ReducedCooldown;
                    break;
                }
            case 3:
                {
                    m_CurrentlyUsedDamage = m_IncreasedDamage;
                    break;
                }
            case 4:
                {
                    m_CurrentlyUsedRadius = m_IncreasedRadius;
                    break;
                }
        }
    }
}