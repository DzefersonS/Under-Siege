using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;

    private AudioSource audioSource = default;
    private Action m_UpdateAction = default;
    private Animator m_Animator = default;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float lerpProgress = 0.0f;
    private float m_Radius = 0.0f;
    private int m_Damage = 0;
    private bool dealtDamage = false;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_UpdateAction.Invoke();
    }

    public void Activate(Vector3 target, float radius, int damage)
    {
        targetPosition = target;
        startPosition = transform.position;
        lerpProgress = 0.0f;
        m_Radius = radius;
        m_Damage = damage;

        transform.rotation = Quaternion.LookRotation(
            Vector3.forward,
            -(targetPosition - transform.position)
        );
        m_UpdateAction = Travel;
    }

    private void Travel()
    {
        lerpProgress += Time.deltaTime * speed;

        transform.position = Vector3.Lerp(startPosition, targetPosition, lerpProgress);

        if (lerpProgress >= 0.75f)
        {
            StartExplosion();
        }
    }

    private void WaitForExplosionFinish()
    {
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Explode") && stateInfo.normalizedTime >= 0f && !dealtDamage)
        {
            Explode();
        }

        if (stateInfo.IsName("Explode") && stateInfo.normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void StartExplosion()
    {
        m_Animator.Play("Explode");
        m_Animator.CrossFade("Explode", 0.0f);
        audioSource.Play();
        m_UpdateAction = WaitForExplosionFinish;
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_Radius);

        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<IAttackable>() != null && col.CompareTag("Enemy"))
            {
                col.GetComponent<IAttackable>().Damage(m_Damage);
            }
        }
        dealtDamage = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
}
