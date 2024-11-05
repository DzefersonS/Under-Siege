using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class DeadBody : Poolable
{
    [SerializeField] private float _timeToSelfDestruct;
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] private Cultist _claimingCultist; //Cultist that will take the body
    private Transform container;

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
            if (_claimingCultist == null || _claimingCultist.isFree == true)
                Unclaim();
        }
    }

    public override void Initialize()
    {
        isClaimed = false;
        _claimingCultist = null;
        _deadBodyEventSO.value = this;
        container = transform.parent;
    }

    public void Claim(Cultist cultist)
    {
        _claimingCultist = cultist;
        isClaimed = true;
    }

    public void Unclaim()
    {
        _claimingCultist = null;
        isClaimed = false;
        _deadBodyEventSO.value = this;

        transform.parent = container;
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