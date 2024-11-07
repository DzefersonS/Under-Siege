using Unity.VisualScripting;
using UnityEngine;

public class InputBasedMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputsSO m_PlayerInputsSO;
    [SerializeField] private Rigidbody2D m_PlayerRB;
    [SerializeField] private Animator m_Animator;

    [SerializeField] private float m_MaxVelocity;
    [SerializeField] private float m_Acceleration;
    [SerializeField] private float m_Decceleration;
    [SerializeField] private float m_JumpForce;
    [SerializeField] private float m_Gravity;
    [SerializeField] private float m_JumpAnimationThreshold = 5f;
    [SerializeField] private float m_FallAnimationThreshold = -2f;

    private Vector2 m_MoveDirection = default;

    private void Update()
    {
        float movingRight = Input.GetKey(m_PlayerInputsSO.moveRight) ? 1.0f : 0.0f;
        float movingLeft = Input.GetKey(m_PlayerInputsSO.moveLeft) ? -1.0f : 0.0f;
        m_MoveDirection = new Vector2(movingLeft + movingRight, m_PlayerRB.velocity.y).normalized;
        if (movingLeft != 0.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0.0f);
        }
        if (movingRight != 0.0f)
        {
            transform.rotation = Quaternion.identity;
        }

        if (Input.GetKey(m_PlayerInputsSO.jump) && m_PlayerRB.velocity.y == 0.0f)
        {
            m_PlayerRB.velocity = new Vector2(m_PlayerRB.velocity.x, m_JumpForce);
        }

        UpdateAnimationStates();
    }

    private void FixedUpdate()
    {
        float newVelocityX = Mathf.Clamp(GetNewVelocityX(Time.fixedDeltaTime), -m_MaxVelocity, m_MaxVelocity);
        float newVelocityY = Mathf.Clamp(GetNewVelocityY(Time.fixedDeltaTime), -m_MaxVelocity, m_MaxVelocity);
        m_PlayerRB.velocity = new Vector2(newVelocityX, newVelocityY);
    }

    private float GetNewVelocityX(float fixedDeltaTime)
    {
        float velocityX = m_PlayerRB.velocity.x;

        if (m_MoveDirection.x != 0.0f)
        {
            velocityX += m_MoveDirection.x * m_Acceleration * fixedDeltaTime;
        }
        else if(velocityX != 0.0f)
        {
            velocityX -= m_Decceleration * fixedDeltaTime * Mathf.Sign(velocityX);
            if (Mathf.Abs(velocityX) < 0.1f)
            {
                velocityX = 0.0f;
            }
        }

        return velocityX;
    }

    private float GetNewVelocityY(float fixedDeltaTime)
    {
        float velocityY = m_PlayerRB.velocity.y;
        velocityY -= m_Gravity * fixedDeltaTime;
        return velocityY;
    }

    private void UpdateAnimationStates()
    {
        bool isMoving = m_MoveDirection.x != 0.0f;
        m_Animator.SetBool("IsRunning", isMoving);

        float verticalVelocity = m_PlayerRB.velocity.y;
        
        if (verticalVelocity > m_JumpAnimationThreshold)
        {
            m_Animator.SetBool("IsJumping", true);
            m_Animator.SetBool("IsFalling", false);
        }
        else if (verticalVelocity < m_FallAnimationThreshold)
        {
            m_Animator.SetBool("IsJumping", false);
            m_Animator.SetBool("IsFalling", true);
        }
        else
        {
            m_Animator.SetBool("IsJumping", false);
            m_Animator.SetBool("IsFalling", false);
        }
    }
}