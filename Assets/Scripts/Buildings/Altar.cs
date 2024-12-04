using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private bool isAccessible; // Currently does nothing, will do something when waves become a thing
    [SerializeField] private UIManager m_UIManager;


    void Start()
    {
        isAccessible = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isAccessible)
        {
            m_UIManager.EnableAltarCanvas();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_UIManager.DisableAltarCanvas();
        }
    }
}
