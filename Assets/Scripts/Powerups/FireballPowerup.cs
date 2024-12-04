using UnityEngine;

public class FireballPowerup : PowerupBase
{
    [SerializeField] private GameObject m_FireballPrefab;
    [SerializeField] private Transform m_FirePoint;
    [SerializeField] private GameObject m_AoeIndicator;

    private bool isAiming = false;
    private Vector3 targetPosition;

    protected override void Start()
    {
        base.Start();
        m_AoeIndicator?.SetActive(false);
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
            fireballScript.SetTarget(targetPosition);
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
}