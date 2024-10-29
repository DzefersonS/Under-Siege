using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveSO[] m_Waves;
    [SerializeField] private float m_TimeBetweenWaves;

    private WaveSO m_CurrentWave = default;
    private Action<float> m_UpdateAction = default;
    private float m_TimeRemaining = 0.0f;
    private int m_CurrentWaveIndex = default;
    private int m_EnemiesAlive = 0;
    private int m_EnemiesLeftToSpawn = 0;

    private void Awake()
    {
        m_TimeRemaining = m_TimeBetweenWaves;
        m_CurrentWaveIndex = 0;
        m_CurrentWave = m_Waves[m_CurrentWaveIndex];
    }

    private void Update()
    {
        m_UpdateAction.Invoke(Time.deltaTime);
    }

    private void DoCountdownBetweenWaves(float deltaTime)
    {
        m_TimeRemaining -= deltaTime;
        if (m_TimeRemaining < 0.0f)
        {
            m_UpdateAction = EnemySpawnTimer;
            m_TimeRemaining += m_CurrentWave.timeBetweenSpawns;
        }
    }

    private void EnemySpawnTimer(float deltaTime)
    {
        m_TimeRemaining -= deltaTime;
        if (m_TimeRemaining < 0.0f)
        {
            // call to spawn enemy
            --m_EnemiesLeftToSpawn;
            ++m_EnemiesAlive;
            if (m_EnemiesLeftToSpawn == 0)
            {
                enabled = false;
            }
            else
            {
                m_TimeRemaining += m_CurrentWave.timeBetweenSpawns;
            }
        }
    }
}