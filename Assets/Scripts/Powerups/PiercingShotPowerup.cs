using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiercingShotPowerup : PowerupBase
{
    [SerializeField] private Player m_Player;
    [SerializeField] private float m_ReducedCooldown = 2.5f;
    [SerializeField] private int m_BasePiercableTargets = 3;
    [SerializeField] private int m_IncreasedPiercableTargets = 5;
    [SerializeField] private float m_BaseRadius = 1.77f;
    [SerializeField] private float m_IncreasedRadius = 2.4f;

    private void Awake()
    {
        m_Player.howManyTargetsPiercable = m_BasePiercableTargets;
    }

    protected override void ActivatePowerup()
    {
        m_Player.ActivatePiercingShot();
        StartCooldown();
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
                    m_Player.howManyTargetsPiercable = m_IncreasedPiercableTargets;
                    break;
                }
            case 4:
                {
                    m_CooldownOverlay.fillAmount = 1.0f;
                    m_CooldownText.text = "∞";
                    m_Player.ActivatePiercingShot(true);
                    enabled = false;
                    break;
                }
        }
    }
}