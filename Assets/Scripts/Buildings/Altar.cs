using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private Canvas _shopCanvas;

    void Start()
    {
        _shopCanvas = GameObject.Find("ShopCanvas").GetComponent<Canvas>();

        if (_shopCanvas != null)
        {
            _shopCanvas.enabled = false;
        }
    }

    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _shopCanvas.enabled = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _shopCanvas.enabled = false;

        }
    }
}
