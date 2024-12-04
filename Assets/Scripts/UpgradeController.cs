using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ShopManager;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private UpgradePurchaseEventSO _upgradePurchaseEventSO;

    [SerializeField] private PlayerDataSO playerData;

    [SerializeField] private FireballPowerup m_FireballPowerup;
    [SerializeField] private PiercingShotPowerup m_PiercingShotPowerup;
    [SerializeField] private DashPowerup m_DashPowerup;
    [SerializeField] private UpgradesSO m_UpgradesSO;

    private int damageUpgradeIndex;
    private int attackSpeedUpgradeIndex;
    private int movementSpeedUpgradeIndex;

    private int defaultDamage;
    private float defaultAttackSpeed;
    private float defaultMoveSpeed;

    private void Awake()
    {
        _upgradePurchaseEventSO.Register(UpgradePlayer);

        damageUpgradeIndex = 0;
        attackSpeedUpgradeIndex = 0;
        movementSpeedUpgradeIndex = 0;
    }

    private void Start()
    {
        defaultDamage = playerData.playerDamage;
        defaultAttackSpeed = playerData.playerAttackSpeed;
        defaultMoveSpeed = playerData.playerSpeed;
    }

    private void OnDestroy()
    {
        _upgradePurchaseEventSO.Unregister(UpgradePlayer);
        ResetValues();
    }

    private void UpgradePlayer()
    {
        int upgradeId = _upgradePurchaseEventSO.value;

        // Upgrade Damage
        if (upgradeId == (int)EUpgradeID.Damage)
        {
            damageUpgradeIndex++;
            playerData.playerDamage = m_UpgradesSO.PlayerDamage[damageUpgradeIndex];
            if (damageUpgradeIndex % 5 == 0)
            {
                m_FireballPowerup.UpgradePowerup();
            }
        }

        // Upgrade Attack Speed
        if (upgradeId == (int)EUpgradeID.AttackSpeed)
        {
            attackSpeedUpgradeIndex++;
            playerData.playerAttackSpeed = m_UpgradesSO.PlayerAttackSpeed[damageUpgradeIndex];
            if (attackSpeedUpgradeIndex % 5 == 0)
            {
                m_PiercingShotPowerup.UpgradePowerup();
            }
        }

        // Upgrade Movement Speed
        if (upgradeId == (int)EUpgradeID.MovementSpeed)
        {
            movementSpeedUpgradeIndex++;
            playerData.playerSpeed = m_UpgradesSO.PlayerMovementSpeed[damageUpgradeIndex];
            if (movementSpeedUpgradeIndex % 5 == 0)
            {
                m_DashPowerup.UpgradePowerup();
            }
        }
    }

    private void ResetValues()
    {
        playerData.playerDamage = defaultDamage;
        playerData.playerAttackSpeed = defaultAttackSpeed;
        playerData.playerSpeed = defaultMoveSpeed;

        damageUpgradeIndex = 0;
        attackSpeedUpgradeIndex = 0;
        movementSpeedUpgradeIndex = 0;
    }
}
