using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CultistManager : MonoBehaviour
{
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] private GameObject _cultistPrefab;
    [SerializeField] private GameObject _cultistParent;
    [SerializeField] private Vector2 _spawnPosition;

    [SerializeField] private List<Cultist> _cultists = new List<Cultist>();
    [SerializeField] private Queue<GameObject> _deadBodies = new Queue<GameObject>();


    private float bodyCheckCooldown = 1f;
    private float nextBodyCheckTime = 0f;


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
        _deadBodies.Enqueue(deadBody.gameObject);
    }

    void Update()
    {
        if (_deadBodies.Count > 0)
            CollectBody();
    }


    public void SpawnCultist()
    {
        if (Random.Range(1, 3) == 1)
            _spawnPosition = new Vector2(12.5f, -2.07f);
        else
            _spawnPosition = new Vector2(8.5f, -2.07f);

        GameObject cultistGameObject = Instantiate(_cultistPrefab, _spawnPosition, Quaternion.identity, _cultistParent.transform);

        Cultist c = cultistGameObject.GetComponent<Cultist>();
        _cultists.Add(c);

    }

    //Finds a free cultist
    private Cultist FindFreeCultist()
    {
        foreach (var cultist in _cultists)
        {
            if (cultist.GetState() == Cultist.State.Idle)
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
        GameObject deadbodyGO = _deadBodies.Peek();//Checking if null

        // Only dequeue a body if a free cultist is available
        if (freeCultist != null && _deadBodies.Count > 0 && deadbodyGO != null)
        {
            DeadBody deadbody = _deadBodies.Dequeue().GetComponent<DeadBody>();

            if (deadbody != null && !deadbody.isClaimed)
            {
                deadbody.Claim(freeCultist.gameObject);
                freeCultist.CollectDeadBody(deadbody.gameObject);
            }
        }
        if (deadbodyGO == null)
        {
            _deadBodies.Dequeue();// remove the null shit from queue.
        }
    }


    //For future implementation.
    private void StartIdling()
    {
        foreach (var cultist in _cultists)
        {
            if (cultist.GetState() == Cultist.State.Idle)
            {
                cultist.Idle();
            }

        }

    }

    private void TerminateCultist()
    {
        //Unclaim a body if he has claimed it.
        //Remove cultist from list
        //If Cultist was carrying a body, maybe drop it?
        //Create a deadbody at cultist location
        //Play death animation.

    }



}

