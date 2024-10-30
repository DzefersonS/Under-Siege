using UnityEngine;
using Utils;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputsSO m_PlayerInputsSO;
    [SerializeField] private PlayerDataSO m_PlayerDataSO;
    [SerializeField] private ObjectPoolSO m_ProjectilePoolSO;
    [SerializeField] private Transform m_PlayerProjectileContainer;
    [SerializeField] private Transform m_ProjectileSpawnAnchor;

    [SerializeField] private ObjectPoolSO _enemyPoolSO;
    [SerializeField] private Transform _enemyContainer;



    private float m_AttackCooldown = 0.0f;

    private void Awake()
    {
        m_ProjectilePoolSO.container = m_PlayerProjectileContainer;
        _enemyPoolSO.container = _enemyContainer; // DELETE THIS 
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
            projectile.Initialize();
            m_AttackCooldown = 1.0f / m_PlayerDataSO.playerAttackSpeed;
        }

        //delete
        if (Input.GetKeyDown(m_PlayerInputsSO.jump))
        {
            Enemy enemy = (Enemy)_enemyPoolSO.GetFreeObject();
            enemy.transform.position = transform.position;
            enemy.transform.rotation = transform.rotation;
            enemy.Initialize();
        }

    }

    private void OnDestroy()
    {
        m_ProjectilePoolSO.DestroyContainer();
        _enemyPoolSO.DestroyContainer();//delete
    }
}