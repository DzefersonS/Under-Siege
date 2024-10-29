using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Upgrades SO", menuName = "Under Siege/Create Upgrades SO")]
public class UpgradesSO : ScriptableObject
{
    [SerializeField] private int[] m_PlayerUpgradePrices;
    [SerializeField] private int[] m_CultistPrices;

    public int[] PlayerUpgradePrices => m_PlayerUpgradePrices;
    public int[] CultistPrices => m_CultistPrices;
}

