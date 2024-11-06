using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cultist Data SO", menuName = "Under Siege/Create Cultist Data SO")]

public class CultistDataSO : ScriptableObject
{
    [SerializeField] private float m_Health;
    [SerializeField] private float m_IdleSpeed;
    [SerializeField] private float m_CollectSpeed;
    [SerializeField] private float m_CarrySpeed;
    [SerializeField] private float m_FleeSpeed;
    [SerializeField] private float m_EnemyDetectionRange;



    public float health { get => m_Health; set => m_Health = value; }
    public float idleSpeed { get => m_IdleSpeed; set => m_IdleSpeed = value; }
    public float collectSpeed { get => m_CollectSpeed; set => m_CollectSpeed = value; }
    public float carrySpeed { get => m_CarrySpeed; set => m_CarrySpeed = value; }
    public float fleeSpeed { get => m_FleeSpeed; set => m_FleeSpeed = value; }
    public float enemyDetectionRange { get => m_EnemyDetectionRange; set => m_EnemyDetectionRange = value; }
}
