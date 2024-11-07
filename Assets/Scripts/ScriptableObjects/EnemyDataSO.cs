using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data SO", menuName = "Under Siege/Create New Enemy Data SO")]
public class EnemyDataSO : ScriptableObject
{
    [SerializeField] private int m_MaxHealth;
    [SerializeField] private float m_AttackRange;
    [SerializeField] private float m_AttackSpeed;
    [SerializeField] private int m_AttackDamage;

    public int maxHealth => m_MaxHealth;
    public float attackRange => m_AttackRange;
    public float attackSpeed => m_AttackSpeed;
    public int attackDamage => m_AttackDamage;
}