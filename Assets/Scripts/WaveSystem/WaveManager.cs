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
    [SerializeField] private EnemySpawner m_EnemySpawner;

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
            m_EnemySpawner.SpawnEnemy(m_CurrentWave.enemies[^m_EnemiesLeftToSpawn]);
            Debug.Log("Spawned enemy!");
            --m_EnemiesLeftToSpawn;
            if (m_EnemiesLeftToSpawn == 0)
            {
                Debug.Log("Finished spawning wave!");
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
        Debug.Log("Enemies alive: " + m_EnemiesAlive);
        var body = m_DeadBodyPoolSO.GetFreeObject();
        body.transform.position = m_EnemyDeathEventSO.value.transform.position;
        body.Initialize();

        if (--m_EnemiesAlive == 0)
        {
            Debug.Log("all enemies ded!");
            if (++m_CurrentWaveIndex >= m_Waves.Length)
            {
                Debug.Log("finished spawning waves!");
                //todo: this is for testing, not a thing we plan to have happen
                return;
            }
            else
            {
                Debug.Log("Starting next wave!");
                m_CurrentWave = m_Waves[m_CurrentWaveIndex];
                m_UpdateAction = DoCountdownBetweenWaves;
                enabled = true;
            }
        }
    }
}