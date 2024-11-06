using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CultistManager : MonoBehaviour
{
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] private GameObject _cultistPrefab;
    [SerializeField] private GameObject _cultistParent;

    [SerializeField] private List<Cultist> _cultists = new List<Cultist>();
    [SerializeField] private Queue<DeadBody> _deadBodies = new Queue<DeadBody>();

    private void Awake()
    {
        _deadBodyEventSO.Register(AddDeadBodyToQueue);
    }
    private void OnDestroy()
    {
        _deadBodyEventSO.Unregister(AddDeadBodyToQueue);
    }

    private void AddDeadBodyToQueue()
    {
        DeadBody deadBody = _deadBodyEventSO.value;
        _deadBodies.Enqueue(deadBody);
    }

    void Update()
    {
        if (_deadBodies.Count > 0)
            CollectBody();
    }


    public void SpawnCultist()
    {
        //between 7 & 13
        //Altar pos is 10, most likely will have to add better way of detecting it
        int minimum = 7;
        int maximum = 13;
        int spawnXCoordinates = Random.Range(minimum, maximum + 1);

        Vector2 spawnPosition = new Vector2(spawnXCoordinates, -2.07f);

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
        GameObject deadbodyGO = _deadBodies.Peek().gameObject;//Checking if null

        // Only dequeue a body if a free cultist is available
        if (freeCultist != null && _deadBodies.Count > 0 && deadbodyGO != null)
        {
            DeadBody deadbody = _deadBodies.Dequeue();

            if (deadbody != null && !deadbody.isClaimed)
            {
                deadbody.Claim(freeCultist);
                freeCultist.ChangeState(Cultist.ECultistState.Collect, deadbody);
            }
        }
        if (deadbodyGO == null)
        {
            _deadBodies.Dequeue();// remove the null shit from queue.
        }
    }

}

