using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Upgrades SO", menuName = "Under Siege/Create Upgrades SO")]
public class UpgradesSO : ScriptableObject
{
    //Prices
    [SerializeField] private int[] m_PlayerUpgradePrices;
    [SerializeField] private int[] m_CultistPrices;
    [SerializeField] private int[] m_ShrinePrices;

    //Values for player
    [SerializeField] private float[] m_PlayerDamageValues;
    [SerializeField] private float[] m_PlayerAttackSpeedValues;
    [SerializeField] private float[] m_PlayerMovementSpeedValues;


    public int[] PlayerUpgradePrices => m_PlayerUpgradePrices;
    public int[] CultistPrices => m_CultistPrices;
    public int[] ShrinePrices => m_ShrinePrices;

    public float[] PlayerDamage => m_PlayerDamageValues;
    public float[] PlayerAttackSpeed => m_PlayerAttackSpeedValues;
    public float[] PlayerMovementSpeed => m_PlayerMovementSpeedValues;

}

