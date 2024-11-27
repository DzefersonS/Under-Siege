using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private UpgradePurchaseEventSO _upgradePurchaseEventSO;

    [SerializeField] private UpgradesSO _upgradesSO; // SO file containing what upgrade player gets with every purchase.
    [SerializeField] private PlayerDataSO playerData;

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

        if (upgradeId == 1 && damageUpgradeIndex < _upgradesSO.PlayerDamage.Length)
        {
            playerData.playerDamage = _upgradesSO.PlayerDamage[damageUpgradeIndex];
            damageUpgradeIndex++;
        }
        if (upgradeId == 2 && attackSpeedUpgradeIndex < _upgradesSO.PlayerAttackSpeed.Length)
        {
            playerData.playerAttackSpeed = _upgradesSO.PlayerAttackSpeed[attackSpeedUpgradeIndex];
            attackSpeedUpgradeIndex++;
        }

        if (upgradeId == 3 && movementSpeedUpgradeIndex < _upgradesSO.PlayerMovementSpeed.Length)
        {
            playerData.playerSpeed = _upgradesSO.PlayerMovementSpeed[movementSpeedUpgradeIndex];
            movementSpeedUpgradeIndex++;
        }
    }

    private void ResetValues()
    {
        playerData.playerDamage = defaultDamage;
        playerData.playerAttackSpeed = defaultAttackSpeed;
        playerData.playerSpeed = defaultMoveSpeed;
    }
}