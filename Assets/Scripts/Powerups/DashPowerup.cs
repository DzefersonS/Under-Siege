using TMPro;
using UnityEngine;

public class DashPowerup : PowerupBase
{
    [SerializeField] private InputBasedMovement m_Player;
    [SerializeField] private Animator m_PlayerAnimator;
    [SerializeField] private float m_DashDistance;
    [SerializeField] private float m_ReducedCooldown;
    [SerializeField] private float m_IncreasedDashDistance;
    [SerializeField] private TMP_Text m_ChargesText;

    private int m_MaxCharges = 1;
    private int m_CurrentCharges = 1;
    private bool isCooldownActive = false;

    private void Awake()
    {
        m_ChargesText.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        HandleCooldown();

        if (Input.GetKeyDown(m_PowerupKeybind))
        {
            if (m_UpgradeLevel == 0)
            {
                m_DeniedActionAudioSource.Play();
            }
            else if (cooldownTimer > 0.0f)
            {
                if (m_CurrentCharges == 1 && m_MaxCharges == 2)
                {
                    ActivatePowerup();
                }
                else
                {
                    m_DeniedActionAudioSource.Play();
                }
            }
            else
            {
                ActivatePowerup();
            }
        }
    }

    protected override void ActivatePowerup()
    {
        if (m_CurrentCharges > 0)
        {
            float direction = m_Player.transform.rotation.y == 0 ? 1 : -1;
            m_Player.transform.position += new Vector3(m_DashDistance * direction, 0.0f, 0.0f);
            // m_PlayerAnimator.Play("Dash");

            m_CurrentCharges--;
            UpdateChargesText();

            if (!isCooldownActive && m_CurrentCharges < m_MaxCharges)
            {
                StartCooldown();
            }
        }
        else
        {
            Debug.LogWarning("No charges available!");
        }
    }

    protected override void HandleCooldown()
    {
        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;

            float cooldownProgress = cooldownTimer / currentlyUsedCooldownDuration;
            m_CooldownOverlay.fillAmount = cooldownProgress;
            m_CooldownText.text = Mathf.Ceil(cooldownTimer).ToString();

            if (cooldownTimer <= 0.0f)
            {
                m_CurrentCharges++;
                UpdateChargesText();

                if (m_CurrentCharges < m_MaxCharges)
                {
                    StartCooldown();
                }
                else
                {
                    ResetCooldownUI();
                    isCooldownActive = false;
                }
            }
        }
    }

    protected override void HandleUpgrade(int level)
    {
        switch (level)
        {
            case 2:
                {
                    currentlyUsedCooldownDuration = m_ReducedCooldown;
                    if (cooldownTimer > m_ReducedCooldown) cooldownTimer = m_ReducedCooldown;
                    break;
                }
            case 3:
                {
                    m_DashDistance = m_IncreasedDashDistance;
                    break;
                }
            case 4:
                {
                    m_MaxCharges = 2;
                    m_CurrentCharges = m_MaxCharges;
                    m_ChargesText.gameObject.SetActive(true);
                    UpdateChargesText();
                    break;
                }
        }
    }

    private void UpdateChargesText()
    {
        m_ChargesText.text = $"{m_CurrentCharges}/{m_MaxCharges}";
    }

    protected override void StartCooldown()
    {
        cooldownTimer = currentlyUsedCooldownDuration;
        m_CooldownOverlay.fillAmount = 1.0f;
        isCooldownActive = true;
    }
}