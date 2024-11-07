using System;
using UnityEngine;
using Utils;

public class PlayerProjectile : Poolable
{
    [SerializeField] private PlayerDataSO m_PlayerDataSO;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_TimeToSelfDestruct;
    [SerializeField] private float m_MovementSpeed;

    private Action m_UpdateAction = default;
    private Vector3 m_MovementVector = default;
    private float m_TimeRemaining = 0.0f;
    private bool m_HitEnemy = false;

    public Vector3 movementVector { set => m_MovementVector = value; }

    public override void Initialize()
    {
        m_TimeRemaining = m_TimeToSelfDestruct;
        m_MovementVector.x *= m_MovementSpeed;
        m_UpdateAction = Fly;
        m_HitEnemy = false;
    }

    private void Update()
    {
        m_UpdateAction.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_HitEnemy)
            return;

        if (other.CompareTag("Enemy"))
        {
            m_HitEnemy = true;
            other.GetComponent<IAttackable>().Damage(m_PlayerDataSO.playerDamage);
            Explode();
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