using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "WaveSO", menuName = "Under Siege/WaveSystem/Create New Wave SO")]
public class WaveSO : ScriptableObject
{
    [SerializeField] private float m_TimeBetweenSpawns;
    [SerializeField] private ObjectPoolSO[] m_Enemies;

    public float timeBetweenSpawns => m_TimeBetweenSpawns;
    public ObjectPoolSO[] enemies => m_Enemies;
}