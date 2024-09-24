using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement2D : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 1.0f;
    [SerializeField] private float m_JumpForce = 5.0f;

    private Rigidbody2D m_RigidBody = default;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            m_RigidBody.velocity = new Vector3(m_MovementSpeed, m_RigidBody.velocity.y);
            transform.rotation = Quaternion.identity;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_RigidBody.velocity = new Vector3(-m_MovementSpeed, m_RigidBody.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector2(0.0f, -180.0f));
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_RigidBody.velocity.y == 0.0f)
        {
            m_RigidBody.velocity = Vector2.up * m_JumpForce;
        }
    }
}