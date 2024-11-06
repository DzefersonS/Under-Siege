using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float m_ParallaxEffect = 0.5f;
    
    private Transform m_CameraTransform;
    private Vector3 m_LastCameraPosition;

    private void Start()
    {
        m_CameraTransform = Camera.main.transform;
        m_LastCameraPosition = m_CameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = m_CameraTransform.position - m_LastCameraPosition;
        transform.position += deltaMovement * m_ParallaxEffect;
        m_LastCameraPosition = m_CameraTransform.position;
    }
}