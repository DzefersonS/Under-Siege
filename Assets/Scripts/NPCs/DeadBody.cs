using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    [SerializeField] private float _timeToSelfDestruct;
    private GameObject _claimingCultist; //Cultist that will take the body

    public bool isClaimed = false;

    private float claimCheckCooldown = 1f;
    private float nextClaimCheckTime = 3f;

    void Start()
    {
        isClaimed = false;
        StartCoroutine(DestroyByTime());
    }

    private void Update()
    {
        if (Time.time >= nextClaimCheckTime)
        {
            nextClaimCheckTime = Time.time + claimCheckCooldown;

            //Check if the cultist is still alive 
            if (_claimingCultist == null)
                Unclaim();
        }
    }


    public void AssignCultist(GameObject cultist)
    {
        _claimingCultist = cultist;
        isClaimed = true; //just for good measure
    }

    public void Unclaim()
    {
        _claimingCultist = null;
        isClaimed = false;

    }

    private IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(_timeToSelfDestruct);

        Destroy(gameObject);
    }

}
