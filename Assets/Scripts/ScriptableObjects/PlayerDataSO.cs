using UnityEngine;

[CreateAssetMenu(fileName = "Player Data SO", menuName = "Under Siege/Create Player Data SO")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private int m_PlayerDamage;
    [SerializeField] private float m_PlayerSpeed;
    [SerializeField] private float m_PlayerAttackSpeed;

    public int playerDamage { get => m_PlayerDamage; set => m_PlayerDamage = value; }
    public float playerSpeed { get => m_PlayerSpeed; set => m_PlayerSpeed = value; }
    public float playerAttackSpeed { get => m_PlayerAttackSpeed; set => m_PlayerAttackSpeed = value; }
}