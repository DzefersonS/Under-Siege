using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cultist Data SO", menuName = "Under Siege/Create Cultist Data SO")]

public class CultistDataSO : ScriptableObject
{
    [SerializeField] private int m_Health;
    [SerializeField] private int m_IdleSpeed;
    [SerializeField] private int m_CollectSpeed;
    [SerializeField] private int m_CarrySpeed;
    [SerializeField] private int m_FleeSpeed;
    [SerializeField] private int m_EnemyDetectionRange;



    public int health { get => m_Health; set => m_Health = value; }
    public int idleSpeed { get => m_IdleSpeed; set => m_IdleSpeed = value; }
    public int collectSpeed { get => m_CollectSpeed; set => m_CollectSpeed = value; }
    public int carrySpeed { get => m_CarrySpeed; set => m_CarrySpeed = value; }
    public int fleeSpeed { get => m_FleeSpeed; set => m_FleeSpeed = value; }
    public int enemyDetectionRange { get => m_EnemyDetectionRange; set => m_EnemyDetectionRange = value; }
}
