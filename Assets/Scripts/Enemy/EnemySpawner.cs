using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ObjectPoolSO[] m_EnemyPoolSOs;

    private void Awake()
    {
        foreach (var pool in m_EnemyPoolSOs)
        {
            pool.container = transform;
        }
    }

    private void OnDestroy()
    {
        foreach (var pool in m_EnemyPoolSOs)
        {
            pool.DestroyContainer();
        }
    }
}