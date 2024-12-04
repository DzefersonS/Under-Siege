using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public abstract class PowerupBase : MonoBehaviour
{
    [SerializeField] protected KeyCode m_PowerupKeybind = default;
    [SerializeField] protected Image m_CooldownOverlay;
    [SerializeField] protected TMP_Text m_CooldownText;
    [SerializeField] private float m_CooldownDuration = 5f;
    [SerializeField] private GameObject m_GrayedOutOverlay;

    protected AudioSource m_DeniedActionAudioSource;
    protected float currentlyUsedCooldownDuration = 0.0f;
    protected float cooldownTimer = 0f;
    protected int m_UpgradeLevel = 0;


    protected virtual void Start()
    {
        m_DeniedActionAudioSource = GetComponent<AudioSource>();
        ResetCooldownUI();
        UpdatePowerupState();
        currentlyUsedCooldownDuration = m_CooldownDuration;
    }

    protected virtual void Update()
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
                m_DeniedActionAudioSource.Play();
            }
            else
            {
                ActivatePowerup();
            }
        }
    }

    protected virtual void HandleCooldown()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;

            float cooldownProgress = cooldownTimer / currentlyUsedCooldownDuration;
            m_CooldownOverlay.fillAmount = cooldownProgress;
            m_CooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
        }
        else
        {
            ResetCooldownUI();
        }
    }

    protected void ResetCooldownUI()
    {
        m_CooldownOverlay.fillAmount = 0f;
        m_CooldownText.text = "";
    }

    protected virtual void StartCooldown()
    {
        cooldownTimer = currentlyUsedCooldownDuration;
        m_CooldownOverlay.fillAmount = 1.0f;
    }

    public void UpgradePowerup()
    {
        m_UpgradeLevel++;

        if (m_UpgradeLevel == 1)
        {
            UpdatePowerupState();
        }
        else if (m_UpgradeLevel <= 4)
        {
            HandleUpgrade(m_UpgradeLevel);
        }
        else
        {
            Debug.LogWarning("Powerup is already at max level!");
        }
    }

    private void UpdatePowerupState()
    {
        bool isUnlocked = m_UpgradeLevel > 0;
        m_GrayedOutOverlay.SetActive(!isUnlocked);
    }

    protected abstract void HandleUpgrade(int level);

    protected abstract void ActivatePowerup();
}
