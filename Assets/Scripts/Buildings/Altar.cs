using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject _shopCanvas;
    [SerializeField] private bool isAccessible; // Currently does nothing, will do something when waves become a thing

    void Start()
    {

        if (_shopCanvas != null)
        {
            _shopCanvas.SetActive(false);
        }

        isAccessible = true;
    }

    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isAccessible)
        {
            _shopCanvas.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _shopCanvas.SetActive(false);

        }
    }
}
