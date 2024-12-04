using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerProjectile : Poolable
{
    [SerializeField] private PlayerDataSO m_PlayerDataSO;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_TimeToSelfDestruct;
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private int m_MaxPiercingTargets;

    private Action m_UpdateAction = default;
    private Vector3 m_MovementVector = default;
    private float m_TimeRemaining = 0.0f;
    private bool m_PiercingShotActive = false;
    private int m_RemainingPiercingHits = 0;
    private HashSet<GameObject> m_HitTargets = new HashSet<GameObject>();

    public Vector3 movementVector { set => m_MovementVector = value; }

    public override void Initialize()
    {
        m_TimeRemaining = m_TimeToSelfDestruct;
        m_MovementVector.x *= m_MovementSpeed;
        m_UpdateAction = Fly;
        m_HitTargets.Clear();
        m_RemainingPiercingHits = m_MaxPiercingTargets;
        m_PiercingShotActive = false;
    }

    public void ActivatePiercingShot()
    {
        m_PiercingShotActive = true;
    }

    private void Update()
    {
        m_UpdateAction.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_UpdateAction == CheckIfExploded)
            return;

        if (m_HitTargets.Contains(other.gameObject))
            return;

        if (other.CompareTag("Enemy"))
        {
            m_HitTargets.Add(other.gameObject);
            other.GetComponent<IAttackable>().Damage(m_PlayerDataSO.playerDamage);

            if (m_PiercingShotActive)
            {
                if (--m_RemainingPiercingHits <= 0)
                {
                    Explode();
                }
            }
            else
            {
                Explode();
            }
        }
    }

    private void Fly()
    {
        m_TimeRemaining -= Time.deltaTime;
        if (m_TimeRemaining < 0.0f)
        {
            Explode();
        }

        transform.position += m_MovementVector * Time.deltaTime;
    }

    private void CheckIfExploded()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            FreeToPool();
        }
    }

    private void Explode()
    {
        m_Animator.Play("ProjectileExplosionAnimation");
        m_UpdateAction = CheckIfExploded;
    }
}
