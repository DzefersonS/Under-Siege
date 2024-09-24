using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer2D : MonoBehaviour
{
    [SerializeField] private RectTransform m_Playbounds;
    [SerializeField] private Transform m_PlayerTransform;
    [SerializeField] private Vector3 m_Offset;
    [SerializeField] private float m_Smoothing;

    private Camera m_Camera = default;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        float halfCameraWidth = m_Camera.orthographicSize * m_Camera.aspect;

        Vector3 boundsMin = m_Playbounds.TransformPoint(m_Playbounds.rect.min); // Bottom-left corner
        Vector3 boundsMax = m_Playbounds.TransformPoint(m_Playbounds.rect.max); // Top-right corner

        Vector3 desiredPos = m_PlayerTransform.position + m_Offset;

        float minX = boundsMin.x + halfCameraWidth;
        float maxX = boundsMax.x - halfCameraWidth;
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, m_Smoothing * Time.deltaTime);
        transform.position = smoothedPos;
    }
}
