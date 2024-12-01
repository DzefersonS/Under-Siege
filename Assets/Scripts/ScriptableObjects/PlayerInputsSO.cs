using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Inputs SO", menuName = "Under Siege/Create Player Inputs SO")]
public class PlayerInputsSO : ScriptableObject
{
    [SerializeField] private KeyCode m_MoveLeft;
    [SerializeField] private KeyCode m_MoveRight;
    [SerializeField] private KeyCode m_Jump;
    [SerializeField] private KeyCode m_Attack;
    [SerializeField] private KeyCode m_Buy;
    //todo: add others when needed

    public KeyCode moveLeft => m_MoveLeft;
    public KeyCode moveRight => m_MoveRight;
    public KeyCode jump => m_Jump;
    public KeyCode attack => m_Attack;
    public KeyCode buy => m_Buy;
}