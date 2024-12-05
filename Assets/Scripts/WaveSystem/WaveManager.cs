using System;
using TMPro;
using UnityEngine;
using Utils;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyEventSO m_EnemyDeathEventSO;
    [SerializeField] private WaveSO[] m_Waves;
    [SerializeField] private float m_TimeBetweenWaves;
    [SerializeField] private EnemySpawner m_EnemySpawner;
    [SerializeField] private TextMeshProUGUI m_WaveText;

    private string m_TimeUntilWaveText => $"Time until next wave: {m_TimeRemainingINT}";
    private string m_EnemiesAliveText => $"Enemies remaining: {m_EnemiesAlive}";

    private WaveSO m_CurrentWave = default;
    private Action<float> m_UpdateAction = default;
    private float m_TimeRemaining = 0.0f;
    private int m_CurrentWaveIndex = default;
    private int m_EnemiesAlive = 0;
    private int m_EnemiesLeftToSpawn = 0;
    private int m_TimeRemainingINT;
    
    private bool m_CanSpawnWaves = false;
    public bool IsWaveInProgress => m_EnemiesAlive > 0;

    private void Awake()
    {
        m_EnemyDeathEventSO.Register(OnEnemyDeath);
        m_TimeRemaining = m_TimeBetweenWaves;
        m_CurrentWaveIndex = 0;
        m_CurrentWave = m_Waves[m_CurrentWaveIndex];
        m_UpdateAction = DoCountdownBetweenWaves;
        
        enabled = false;
    }

    public void EnableWaveSpawning()
    {
        m_CanSpawnWaves = true;
        enabled = true;
    }

    public void DisableWaveSpawning()
    {
        m_CanSpawnWaves = false;
        if (!IsWaveInProgress)
        {
            enabled = false;
        }
    }

    public int GetCurrentWaveIndex()
    {
        return m_CurrentWaveIndex;
    }

    private void Update()
    {
        m_UpdateAction.Invoke(Time.deltaTime);
    }

    private void OnDestroy()
    {
        m_EnemyDeathEventSO.Unregister(OnEnemyDeath);
    }

    private void DoCountdownBetweenWaves(float deltaTime)
    {
        if (!m_CanSpawnWaves)
        {
            m_WaveText.text = "Wave spawning paused";
            return;
        }

        m_TimeRemaining -= deltaTime;
        m_TimeRemainingINT = (int)m_TimeRemaining;
        m_WaveText.text = m_TimeUntilWaveText;
        
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
        m_WaveText.text = m_EnemiesAliveText;
        m_TimeRemaining -= deltaTime;
        if (m_TimeRemaining < 0.0f)
        {
            m_EnemySpawner.SpawnEnemy(m_CurrentWave.enemies[^m_EnemiesLeftToSpawn]);
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
        if (m_UpdateAction == DoCountdownBetweenWaves)
        {
            return;
        }

        if (--m_EnemiesAlive == 0)
        {
            if (++m_CurrentWaveIndex >= m_Waves.Length)
            {
                return;
            }
            else
            {
                m_CurrentWave = m_Waves[m_CurrentWaveIndex];
                m_UpdateAction = DoCountdownBetweenWaves;
                m_TimeRemaining = m_TimeBetweenWaves;
                enabled = true;
            }
        }
        else
        {
            m_WaveText.text = m_EnemiesAliveText;
        }
    }
}