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

    private void Awake()
    {
        _upgradePurchaseEventSO.Register(UpgradePlayer);
        damageUpgradeIndex = 0;
        attackSpeedUpgradeIndex = 0;
        movementSpeedUpgradeIndex = 0;
    }

    private void OnDestroy()
    {
        _upgradePurchaseEventSO.Unregister(UpgradePlayer);
    }


    private void UpgradePlayer()
    {
        int upgradeId = _upgradePurchaseEventSO.value;
        Debug.Log($"{upgradeId}");


        if (upgradeId == 1)
        {
            //playerData.playerDamage = _upgradesSO.PlayerDamage[DamageUpgradeIndex];
            damageUpgradeIndex++;
        }
        if (upgradeId == 2)
        {
            Debug.Log($"Old Speed: {playerData.playerAttackSpeed}");

            playerData.playerAttackSpeed = _upgradesSO.PlayerAttackSpeed[attackSpeedUpgradeIndex];
            attackSpeedUpgradeIndex++;
            Debug.Log($"New Speed: {playerData.playerAttackSpeed}");

        }

        if (upgradeId == 3)
        {
            playerData.playerSpeed = _upgradesSO.PlayerMovementSpeed[movementSpeedUpgradeIndex];
            movementSpeedUpgradeIndex++;
        }
    }

}
