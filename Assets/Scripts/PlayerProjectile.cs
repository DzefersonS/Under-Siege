using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerProjectile : Poolable
{
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private PlayerDataSO m_PlayerDataSO;
    [SerializeField] private Animator m_Animator;

    public Rigidbody2D rigidBody => m_Rigidbody;

    public override void Initialize()
    {
        m_Rigidbody.simulated = true;
        enabled = false;
    }

    private void Update()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            FreeToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(m_PlayerDataSO.playerDamage);
            Explode();
        }
    }

    private void Explode()
    {
        m_Rigidbody.simulated = false;
        m_Animator.Play("ProjectileExplosionAnimation");
        enabled = true;
    }
}