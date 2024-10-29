using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistManager : MonoBehaviour
{

    [SerializeField] private List<Cultist> _cultists = new List<Cultist>();
    [SerializeField] private Queue<GameObject> _deadBodies = new Queue<GameObject>();

    [SerializeField] private GameObject _cultistPrefab;
    [SerializeField] private GameObject _cultistParent;
    [SerializeField] private Vector2 _spawnPosition;

    private float bodyCheckCooldown = 1f;
    private float nextBodyCheckTime = 0f;

    void Update()
    {
        if (Time.time >= nextBodyCheckTime)
        {
            FindDeadBodies();
            nextBodyCheckTime = Time.time + bodyCheckCooldown;
        }
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

    //Adds bodies to the list
    private Queue<GameObject> FindDeadBodies()
    {
        Collider2D[] bodiesInRange = Physics2D.OverlapCircleAll(transform.position, 9999);

        foreach (Collider2D body in bodiesInRange)
        {
            // Check if the collider is tagged "DeadBody"
            if (body.CompareTag("DeadBody") && !_deadBodies.Contains(body.gameObject))
            {
                _deadBodies.Enqueue(body.gameObject);
            }
        }

        return _deadBodies;
    }
    //Finds a free cultist
    private Cultist FindFreeCultist()
    {
        foreach (var cultist in _cultists)
        {
            if (!cultist.IsBusy())
            {
                Debug.Log("Found a cultist" + cultist.IsBusy());
                return cultist;
            }
        }
        return null;
    }

    private GameObject PopFreeDeadBody()
    {
        return _deadBodies.Dequeue();
    }

    //Sends cultist to collect a body
    private void CollectBody()
    {
        GameObject deadbody = PopFreeDeadBody();
        Cultist c = FindFreeCultist();
        // _deadBodies.Remove(deadbody);

        if (c != null && deadbody != null)
        {
            c.CollectDeadBody(deadbody);
        }
        else
            Debug.Log("Deadbody:" + deadbody + ", Cultist: " + c + ", DeadBody count: " + _deadBodies.Count);
    }


}
