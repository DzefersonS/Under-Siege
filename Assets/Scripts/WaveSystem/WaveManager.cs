using System;
using UnityEngine;
using Utils;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;
    [SerializeField] private WaveSO[] m_Waves;
    [SerializeField] private float m_TimeBetweenWaves;
    [SerializeField] private Transform m_DeadBodyContainer;
    [SerializeField] private ObjectPoolSO m_DeadBodyPoolSO;

    private WaveSO m_CurrentWave = default;
    private Action<float> m_UpdateAction = default;
    private float m_TimeRemaining = 0.0f;
    private int m_CurrentWaveIndex = default;
    private int m_EnemiesAlive = 0;
    private int m_EnemiesLeftToSpawn = 0;

    private void Awake()
    {
        m_EnemyDeathEventSO.Register(OnEnemyDeath);
        m_TimeRemaining = m_TimeBetweenWaves;
        m_CurrentWaveIndex = 0;
        m_CurrentWave = m_Waves[m_CurrentWaveIndex];
        m_UpdateAction = DoCountdownBetweenWaves;
        m_DeadBodyPoolSO.container = m_DeadBodyContainer;
    }

    private void Update()
    {
        m_UpdateAction.Invoke(Time.deltaTime);
    }

    private void OnDestroy()
    {
        m_EnemyDeathEventSO.Unregister(OnEnemyDeath);
        m_DeadBodyPoolSO.DestroyContainer();
    }

    private void DoCountdownBetweenWaves(float deltaTime)
    {
        m_TimeRemaining -= deltaTime;
        if (m_TimeRemaining < 0.0f)
        {
            m_UpdateAction = EnemySpawnTimer;
            m_EnemiesAlive = m_CurrentWave.enemies.Length;
            m_EnemiesLeftToSpawn = m_EnemiesAlive;
            m_TimeRemaining += m_CurrentWave.timeBetweenSpawns;
        }
    }

    private void EnemySpawnTimer(float deltaTime)
    {
        m_TimeRemaining -= deltaTime;
        if (m_TimeRemaining < 0.0f)
        {
            Enemy enemy = (Enemy)m_CurrentWave.enemies[^m_EnemiesLeftToSpawn].GetFreeObject();
            enemy.transform.position = Vector3.zero;
            enemy.Initialize();
            --m_EnemiesLeftToSpawn;
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

    private void OnEnemyDeath()
    {
        var body = m_DeadBodyPoolSO.GetFreeObject();
        body.transform.position = m_EnemyDeathEventSO.value.transform.position;
        body.Initialize();

        if (--m_EnemiesAlive == 0)
        {
            if (++m_CurrentWaveIndex >= m_Waves.Length)
            {
                //todo: this is for testing, not a thing we plan to have happen
                return;
            }
            else
            {
                m_CurrentWave = m_Waves[m_CurrentWaveIndex];
                m_UpdateAction = DoCountdownBetweenWaves;
                enabled = true;
            }
        }
    }
}