using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    private ShopManager _shopManager;

    void Start()
    {
        _shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DeadBody")
        {
            _shopManager.AddSouls(1);
            other.GetComponent<DeadBody>().FreeToPool();
        }
    }


}
