using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingShotPowerup : PowerupBase
{
    [SerializeField] private Player m_Player;

    protected override void ActivatePowerup()
    {
        m_Player.ActivatePiercingShot();
        StartCooldown();
    }

    protected override void HandleUpgrade(int level)
    {
        throw new System.NotImplementedException();
    }
}