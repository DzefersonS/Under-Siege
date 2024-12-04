using System;
using UnityEngine;
using Utils;

[RequireComponent(typeof(InputBasedMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputBasedMovement m_PlayerMovement;
    [SerializeField] private PlayerInputsSO m_PlayerInputsSO;
    [SerializeField] private PlayerDataSO m_PlayerDataSO;
    [SerializeField] private ObjectPoolSO m_ProjectilePoolSO;
    [SerializeField] private Transform m_PlayerProjectileContainer;
    [SerializeField] private Transform m_ProjectileSpawnAnchor;
    [SerializeField] private Animator m_Animator;

    private Action m_UpdateAction = default;
    private float m_AttackCooldown = 0.0f;
    private bool m_SpawnedProjectile = false;

    private void Awake()
    {
        m_ProjectilePoolSO.container = m_PlayerProjectileContainer;
        m_PlayerMovement = GetComponent<InputBasedMovement>();
        m_UpdateAction = NotAttacking;
    }

    private void Update()
    {
        m_UpdateAction.Invoke();
    }

    private void NotAttacking()
    {
        if (m_AttackCooldown > 0.0f)
        {
            m_AttackCooldown -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(m_PlayerInputsSO.attack) && m_PlayerMovement.isGrounded)
        {
            m_Animator.Play("Attack");
            m_UpdateAction = WaitForAttackAnimationFinish;
            m_PlayerMovement.EnableInput(false);
        }
    }

    private void WaitForAttackAnimationFinish()
    {
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 0.85f && !m_SpawnedProjectile)
        {
            SpawnProjectile();
            m_SpawnedProjectile = true;
        }
        else if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            m_AttackCooldown = 1.0f / m_PlayerDataSO.playerAttackSpeed;
            m_Animator.Play("Idle");
            m_UpdateAction = NotAttacking;
            m_SpawnedProjectile = false;
            m_PlayerMovement.EnableInput(true);
        }
    }

    private void SpawnProjectile()
    {
        PlayerProjectile projectile = (PlayerProjectile)m_ProjectilePoolSO.GetFreeObject();
        projectile.transform.position = m_ProjectileSpawnAnchor.position;
        projectile.transform.rotation = transform.rotation;
        float movementDirectionX = transform.rotation.eulerAngles.y > 0 ? -1 : 1;
        projectile.movementVector = new Vector2(movementDirectionX, 0.0f);
        projectile.Initialize();
    }

    private void OnDestroy()
    {
        m_ProjectilePoolSO.DestroyContainer();
    }
}