using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public abstract class PowerupBase : MonoBehaviour
{
    [SerializeField] protected KeyCode m_PowerupKeybind = default;
    [SerializeField] private Image m_CooldownOverlay;
    [SerializeField] private TMP_Text m_CooldownText;
    [SerializeField] private float m_CooldownDuration = 5f;

    protected float cooldownTimer = 0f;
    protected AudioSource m_DeniedActionAudioSource;

    private void Awake()
    {
        m_DeniedActionAudioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        ResetCooldownUI();
    }

    protected virtual void Update()
    {
        HandleCooldown();

        // Input handling for activation
        if (Input.GetKeyDown(m_PowerupKeybind))
        {
            if (cooldownTimer > 0.0f)
            {
                m_DeniedActionAudioSource?.Play();
            }
            else
            {
                ActivatePowerup();
            }
        }
    }

    private void HandleCooldown()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;

            float cooldownProgress = cooldownTimer / m_CooldownDuration;
            m_CooldownOverlay.fillAmount = cooldownProgress;
            m_CooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
        }
        else
        {
            ResetCooldownUI();
        }
    }

    private void ResetCooldownUI()
    {
        m_CooldownOverlay.fillAmount = 0f;
        m_CooldownText.text = "";
    }

    protected void StartCooldown()
    {
        cooldownTimer = m_CooldownDuration;
        m_CooldownOverlay.fillAmount = 1.0f;
    }

    protected abstract void ActivatePowerup();
}