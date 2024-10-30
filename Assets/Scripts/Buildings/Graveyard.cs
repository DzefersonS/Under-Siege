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

    private void OnTriggerEnter2D(Collider2D other)// not done
    {
        if (other.tag == "Cultist")
        {
            if (other.GetComponent<Cultist>().IsCarryingBody)

                _shopManager.AddSouls(1);


        }
    }

    private void OnTriggerExit2D(Collider2D other)// not done
    {
        if (other.tag == "Cultist")
        {
            if (other.GetComponent<Cultist>().IsCarryingBody)

                other.transform.GetChild(0).gameObject.SetActive(false);//Disable the deadbody object

        }
    }
}
