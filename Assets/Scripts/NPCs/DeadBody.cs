using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class DeadBody : Poolable
{
    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;
    [SerializeField] private Cultist _claimingCultist; //Cultist that will take the body
    private Transform container;

    public bool isClaimed = false;

    private float claimCheckCooldown = 1f;
    private float nextClaimCheckTime = 3f;

    void Start()
    {
        isClaimed = false;
    }

    private void Update()
    {
        if (Time.time >= nextClaimCheckTime)
        {
            nextClaimCheckTime = Time.time + claimCheckCooldown;

            //Check if the cultist is still alive 
            if (_claimingCultist == null || _claimingCultist.m_CurrentState == Cultist.ECultistState.Idle)
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
        transform.parent = container;

        _claimingCultist = null;
        isClaimed = false;
        _deadBodyEventSO.value = this;
    }

    public Cultist GetClaimant()
    {
        return _claimingCultist;
    }

}