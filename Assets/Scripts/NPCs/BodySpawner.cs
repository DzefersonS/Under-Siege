using UnityEngine;
using Utils;

public class BodySpawner : MonoBehaviour
{
    [SerializeField] private Transform m_DeadBodyContainer;
    [SerializeField] private ObjectPoolSO m_DeadBodyPoolSO;
    [SerializeField] private TransformEventSO m_BodySpawnPositionSO;

    private void Awake()
    {
        m_DeadBodyPoolSO.container = m_DeadBodyContainer;
        m_BodySpawnPositionSO.Register(SpawnBody);
    }

    private void OnDestroy()
    {
        m_DeadBodyPoolSO.DestroyContainer();
        m_BodySpawnPositionSO.Unregister(SpawnBody);
    }

    private void SpawnBody()
    {
        var body = m_DeadBodyPoolSO.GetFreeObject();
        body.transform.position = m_BodySpawnPositionSO.value.position;
        body.Initialize();
    }
}