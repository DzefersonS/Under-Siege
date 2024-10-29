using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    [SerializeField] private ShopManager _shopManager;
    void Start()
    {
        _shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();

    }

    private void OnTriggerEnter2D(Collider2D other)// not done
    {
        if (other.tag == "Cultist")
            _shopManager.AddSouls(1);
    }
}
