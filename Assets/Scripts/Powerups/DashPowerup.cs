using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPowerup : PowerupBase
{
    [SerializeField] private InputBasedMovement m_Player;
    [SerializeField] private Animator m_PlayerAnimator;
    [SerializeField] private float m_DashDistance;

    protected override void ActivatePowerup()
    {
        float direction = m_Player.transform.rotation.y == 0 ? 1 : -1;
        m_Player.transform.position += new Vector3(m_DashDistance * direction, 0.0f, 0.0f);
        // m_PlayerAnimator.Play("Dash");

        StartCooldown();
    }

    protected override void HandleUpgrade(int level)
    {
        throw new System.NotImplementedException();
    }
}
