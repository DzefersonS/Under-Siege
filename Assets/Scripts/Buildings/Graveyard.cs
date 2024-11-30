using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private DeadBodyDeliveredEventSO _deadBodyDeliveredEventSO;

    void Start()
    {
        if (_shopManager == null)
        {
            Debug.LogWarning("ShopManager is null");
            _shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DeadBody")
        {
            _shopManager.AddSouls(1);
            _deadBodyDeliveredEventSO.value = other.GetComponent<DeadBody>();

            other.GetComponent<DeadBody>().FreeToPool();
        }
    }


}
