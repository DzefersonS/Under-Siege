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

    /* Currently known bug: If deadbody appears on Graveyards location, when cultist picks it up, it will not register as delivered */

    private void OnTriggerEnter2D(Collider2D other)// not done
    {
        if (other.tag == "Cultist")
        {
            if (other.GetComponent<Cultist>().isCarryingBody)

                _shopManager.AddSouls(1);


        }
    }

    private void OnTriggerExit2D(Collider2D other)// not done
    {
        if (other.tag == "Cultist")
        {
            if (other.GetComponent<Cultist>().isCarryingBody)

                other.transform.GetChild(0).gameObject.SetActive(false);//Disable the deadbody object

        }
    }
}
