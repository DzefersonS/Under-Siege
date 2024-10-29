using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerProjectile : Poolable
{
    [SerializeField] private Rigidbody2D m_Rigidbody;

    public Rigidbody2D rigidBody => m_Rigidbody;
}