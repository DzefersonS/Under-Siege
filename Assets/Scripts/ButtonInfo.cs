using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public int itemID;
    public TMP_Text PriceTxt;
    public TMP_Text QuantityTxt;
    public GameObject ShopManager;
    void Update()
    {
        PriceTxt.text = "Price: " + ShopManager.GetComponent<ShopManager>().shopItems[2, itemID].ToString();
        QuantityTxt.text = ShopManager.GetComponent<ShopManager>().shopItems[3, itemID].ToString();

    }
}