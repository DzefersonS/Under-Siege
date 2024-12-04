using UnityEngine;

public class InputBasedMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputsSO m_PlayerInputsSO;
    [SerializeField] private Animator m_Animator;

    [SerializeField] private float m_MaxVelocity = 10.0f;
    [SerializeField] private float m_Acceleration = 20.0f;
    [SerializeField] private float m_Decceleration = 15.0f;
    [SerializeField] private float m_JumpForce = 7.5f;
    [SerializeField] private float m_Gravity = 15.0f;
    [SerializeField] private float m_JumpAnimationThreshold = 5.0f;
    [SerializeField] private float m_FallAnimationThreshold = -2.0f;
    [SerializeField] private float m_GroundLevel = -2.8574f;

    private Vector2 m_Velocity = Vector2.zero;
    private Vector2 m_MoveDirection = Vector2.zero;
    private bool m_IsGrounded = true;
    private bool m_InputEnabled = true;

    public bool isGrounded => m_IsGrounded;

    public void EnableInput(bool enable)
    {
        m_InputEnabled = enable;
        if (!enable)
        {
            m_MoveDirection = Vector2.zero;
            m_Velocity = Vector2.zero;
            UpdateAnimationStates();
        }
    }

    private void Update()
    {
        if (!m_InputEnabled) return;

        float movingRight = Input.GetKey(m_PlayerInputsSO.moveRight) ? 1.0f : 0.0f;
        float movingLeft = Input.GetKey(m_PlayerInputsSO.moveLeft) ? -1.0f : 0.0f;
        m_MoveDirection = new Vector2(movingLeft + movingRight, 0f).normalized;

        if (movingLeft != 0.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0.0f);
        }
        if (movingRight != 0.0f)
        {
            transform.rotation = Quaternion.identity;
        }

        if (Input.GetKey(m_PlayerInputsSO.jump) && m_IsGrounded)
        {
            m_Velocity.y = m_JumpForce;
            m_IsGrounded = false;
        }

        UpdateAnimationStates();
    }

    private void FixedUpdate()
    {
        float targetVelocityX = m_MoveDirection.x * m_MaxVelocity;
        if (m_MoveDirection.x != 0f)
        {
            m_Velocity.x = Mathf.MoveTowards(m_Velocity.x, targetVelocityX, m_Acceleration * Time.fixedDeltaTime);
        }
        else
        {
            m_Velocity.x = Mathf.MoveTowards(m_Velocity.x, 0f, m_Decceleration * Time.fixedDeltaTime);
        }

        if (!m_IsGrounded)
        {
            m_Velocity.y -= m_Gravity * Time.fixedDeltaTime;
        }

        Vector2 newPosition = (Vector2)transform.position + m_Velocity * Time.fixedDeltaTime;
        transform.position = newPosition;

        if (transform.position.y <= m_GroundLevel)
        {
            m_IsGrounded = true;
            m_Velocity.y = 0f;
            transform.position = new Vector2(transform.position.x, m_GroundLevel);
        }
    }

    private void UpdateAnimationStates()
    {
        bool isMoving = Mathf.Abs(m_MoveDirection.x) > 0.1f;
        m_Animator.SetBool("IsRunning", isMoving);

        float verticalVelocity = m_Velocity.y;

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
