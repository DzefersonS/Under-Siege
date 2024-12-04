using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ObjectPoolSO[] m_EnemyPoolSOs;
    [SerializeField] private RectTransform m_Playbounds;

    [Serializable]
    private struct SpawnPos
    {
        [SerializeField] private Transform m_SpawnPoint;
        [SerializeField] private int m_DefaultWeight;
        [SerializeField] private int m_WeightGrowthRate;

        private int m_CurrentWeight;
        private Vector2 m_Position;

        public int defaultWeight => m_DefaultWeight;
        public int weightGrowthRate => m_WeightGrowthRate;
        public int currentWeight { get => m_CurrentWeight; set => m_CurrentWeight = value; }
        public Vector2 position => m_SpawnPoint.position;
    }

    public enum ESpawnPos
    {
        Left = 0,
        Right,
        COUNT
    }

    [SerializeField] private SpawnPos[] m_SpawnPositions;

    private void Awake()
    {
        foreach (var pool in m_EnemyPoolSOs)
        {
            pool.container = transform;
        }

        Vector3 boundsMin = m_Playbounds.TransformPoint(m_Playbounds.rect.min); // Bottom-left corner
        Vector3 boundsMax = m_Playbounds.TransformPoint(m_Playbounds.rect.max); // Top-right corner

        m_SpawnPositions[(int)ESpawnPos.Left].currentWeight = m_SpawnPositions[(int)ESpawnPos.Left].defaultWeight;

        m_SpawnPositions[(int)ESpawnPos.Right].currentWeight = m_SpawnPositions[(int)ESpawnPos.Right].defaultWeight;
    }

    private void OnDestroy()
    {
        foreach (var pool in m_EnemyPoolSOs)
        {
            pool.DestroyContainer();
        }
    }

    public void SpawnEnemy(ObjectPoolSO pool)
    {
        int sum = m_SpawnPositions[(int)ESpawnPos.Left].currentWeight + m_SpawnPositions[(int)ESpawnPos.Right].currentWeight;
        int seed = UnityEngine.Random.Range(0, sum);

        Vector2 enemyPos;
        int spawnPosIndex;

        if (seed < m_SpawnPositions[(int)ESpawnPos.Left].currentWeight)
        {
            spawnPosIndex = (int)ESpawnPos.Left;
        }
        else
        {
            spawnPosIndex = (int)ESpawnPos.Right;
        }

        enemyPos = m_SpawnPositions[spawnPosIndex].position;
        m_SpawnPositions[spawnPosIndex].currentWeight = m_SpawnPositions[spawnPosIndex].defaultWeight;
        m_SpawnPositions[1 - spawnPosIndex].currentWeight += m_SpawnPositions[1 - spawnPosIndex].weightGrowthRate;

        Enemy enemy = (Enemy)pool.GetFreeObject();
        Bounds bounds = enemy.GetComponent<SpriteRenderer>().bounds;
        float spriteBottomOffset = bounds.extents.y;
        enemyPos.y += spriteBottomOffset;
        enemy.transform.position = enemyPos;
        enemy.Initialize();
    }
}