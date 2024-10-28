using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject _shopCanvas;

    void Start()
    {

        if (_shopCanvas != null)
        {
            _shopCanvas.SetActive(false);
        }
    }

    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
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
