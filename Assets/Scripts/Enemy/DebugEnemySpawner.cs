using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class DebugEnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawner m_EnemySpawner;
    [SerializeField] private ObjectPoolSO[] m_EnemyPoolSOs;
    [SerializeField] private EEnemyType m_EnemyTypeToSpawn;
    [SerializeField] private KeyCode m_EnemySpawnKey;

    public enum EEnemyType
    {
        Swordsman,
        Spearman,
        Rifleman,
        Brute
    };

    private void Update()
    {
        if (Input.GetKeyDown(m_EnemySpawnKey))
        {
            SpawnEnemy(m_EnemyTypeToSpawn);
        }
    }

    public void SpawnEnemy(EEnemyType enemyType)
    {
        m_EnemySpawner.SpawnEnemy(m_EnemyPoolSOs[(int)enemyType]);
    }

    public void SpawnEnemy(Vector3 location)
    {
        SpawnEnemy(location, m_EnemyTypeToSpawn);
    }

    public void SpawnEnemy(Vector3 location, EEnemyType enemyType)
    {
        Enemy enemy = (Enemy)m_EnemyPoolSOs[(int)enemyType].GetFreeObject();
        enemy.transform.position = location;
        enemy.Initialize();
    }
}