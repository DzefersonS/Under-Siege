using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class DeadBody : Poolable
{
    [SerializeField] private float _timeToSelfDestruct;
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] private Cultist _claimingCultist; //Cultist that will take the body

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

    public override void Initialize()
    {
        isClaimed = false;
        _claimingCultist = null;
        _deadBodyEventSO.value = this;
    }

    public void Claim(Cultist cultist)
    {
        _claimingCultist = cultist;
        isClaimed = true; //just for good measure
    }

    public void Unclaim()
    {
        _claimingCultist = null;
        isClaimed = false;
    }

    public Cultist GetClaimant()
    {
        return _claimingCultist;
    }

    private IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(_timeToSelfDestruct);
        FreeToPool();
    }

}