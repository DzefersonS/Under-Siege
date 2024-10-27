using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Upgrades SO", menuName = "Under Siege/Create Upgrades SO")]
public class UpgradesSO : ScriptableObject
{
    [SerializeField] private int[] m_DamageUpgradePrices;
    [SerializeField] private int[] m_AttackSpeedUpgradePrices;
    [SerializeField] private int[] m_MovementSpeedUpgradePrices;

    public int[] DamageUpgradePrices => m_DamageUpgradePrices;
    public int[] AttackSpeedUpgradePrices => m_AttackSpeedUpgradePrices;
    public int[] MovementSpeedUpgradePrices => m_MovementSpeedUpgradePrices;
}

