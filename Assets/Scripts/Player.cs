using UnityEngine;
using Utils;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputsSO m_PlayerInputsSO;
    [SerializeField] private PlayerDataSO m_PlayerDataSO;
    [SerializeField] private ObjectPoolSO m_ProjectilePoolSO;
    [SerializeField] private Transform m_PlayerProjectileContainer;
    [SerializeField] private Transform m_ProjectileSpawnAnchor;

    private float m_AttackCooldown = 0.0f;

    private void Awake()
    {
        m_ProjectilePoolSO.container = m_PlayerProjectileContainer;
    }

    private void Update()
    {
        if (m_AttackCooldown > 0.0f)
        {
            m_AttackCooldown -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(m_PlayerInputsSO.attack))
        {
            PlayerProjectile projectile = (PlayerProjectile)m_ProjectilePoolSO.GetFreeObject();
            projectile.transform.position = m_ProjectileSpawnAnchor.position;
            projectile.transform.rotation = transform.rotation;
            //dirty code, fix later
            projectile.rigidBody.velocity = new Vector3(4.0f * (transform.rotation.eulerAngles.y > 0 ? -1 : 1), 0.0f, 0.0f);
            m_AttackCooldown = 1.0f / m_PlayerDataSO.playerAttackSpeed;
        }
    }

    private void OnDestroy()
    {
        m_ProjectilePoolSO.DestroyContainer();
    }
}