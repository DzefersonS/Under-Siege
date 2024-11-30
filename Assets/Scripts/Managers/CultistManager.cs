using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class CultistManager : MonoBehaviour
{
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] private CultistEventSO _cultistDeathEventSO;
    [SerializeField] private DeadBodyDeliveredEventSO _deadBodyDeliveredEventSO;
    [SerializeField] private ObjectPoolSO m_DeadBodyPoolSO;


    [SerializeField] private GameObject _cultistPrefab;
    [SerializeField] private GameObject _cultistParent;

    [SerializeField] private List<Cultist> _cultists = new List<Cultist>();
    [SerializeField] private Queue<DeadBody> _deadBodies = new Queue<DeadBody>();

    [SerializeField] private int CultistSpawnPositionXMinimum;
    [SerializeField] private int CultistSpawnPositionXMaximum;
    [SerializeField] private float CultistSpawnPositionY;

    [SerializeField] public int deadbodycount;

    private float interval = 0.15f;
    private float nextTime = 0f;


    private void Awake()
    {
        _deadBodyEventSO.Register(AddDeadBodyToQueue);
        _cultistDeathEventSO.Register(OnCultistDeath);
        _deadBodyDeliveredEventSO.Register(DeadBodyDelivered);
    }
    private void OnDestroy()
    {
        _deadBodyEventSO.Unregister(AddDeadBodyToQueue);
        _cultistDeathEventSO.Unregister(OnCultistDeath);
        _deadBodyDeliveredEventSO.Unregister(DeadBodyDelivered);

    }

    private void AddDeadBodyToQueue()
    {
        deadbodycount = _deadBodies.Count;

        DeadBody deadBody = _deadBodyEventSO.value;
        if (deadBody != null && !_deadBodies.Contains(deadBody))
            _deadBodies.Enqueue(deadBody);
    }

    void Update()
    {
        if (_deadBodies.Count > 0 && Time.time >= nextTime)
        {
            CollectBody();
            nextTime += interval;
        }
    }

    public void SpawnCultist()
    {
        int spawnXCoordinates = Random.Range(CultistSpawnPositionXMinimum, CultistSpawnPositionXMaximum);

        Vector2 spawnPosition = new Vector2(spawnXCoordinates, CultistSpawnPositionY);

        GameObject cultistGameObject = Instantiate(_cultistPrefab, spawnPosition, Quaternion.identity, _cultistParent.transform);

        Cultist c = cultistGameObject.GetComponent<Cultist>();
        _cultists.Add(c);

    }

    //Finds a free cultist
    private Cultist FindFreeCultist()
    {
        foreach (var cultist in _cultists)
        {
            if (cultist.m_CurrentState == Cultist.ECultistState.Idle)
            {
                return cultist;
            }
        }
        return null;
    }

    //Sends cultist to collect a body
    private void CollectBody()
    {
        Cultist freeCultist = FindFreeCultist();

        _deadBodies.TryPeek(out DeadBody deadbodyGO);

        if (deadbodyGO == null)
        {
            _deadBodies.Dequeue();// remove the null shit from queue.
        }


        // Only dequeue a body if a free cultist is available
        if (freeCultist != null && _deadBodies.Count > 0)
        {
            DeadBody deadbody = _deadBodies.Dequeue();

            if (deadbody != null && !deadbody.isClaimed)
            {
                deadbody.Claim(freeCultist);
                freeCultist.ChangeState(Cultist.ECultistState.Collect, deadbody);
            }
        }
    }

    private void OnCultistDeath()
    {
        Cultist c = _cultistDeathEventSO.value;

        if (c.deadBody != null)
            c.deadBody.Unclaim();

        _cultists.Remove(c);

        var body = m_DeadBodyPoolSO.GetFreeObject();
        body.transform.position = c.transform.position;
        body.Initialize();
    }

    private void DeadBodyDelivered()
    {
        DeadBody deliveredBody = _deadBodyDeliveredEventSO.value;
        _deadBodies = new Queue<DeadBody>(_deadBodies.Where(x => x != deliveredBody));
    }

}

