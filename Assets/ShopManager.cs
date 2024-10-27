using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[5, 5];
    [SerializeField] private UpgradesSO _upgradePrices;
    public float souls;
    public TMP_Text SoulsTxt;


    void Start()
    {
        SoulsTxt.text = souls.ToString();

        //Item ID's
        shopItems[1, 1] = 1; //dmg
        shopItems[1, 2] = 2; //attack speed
        shopItems[1, 3] = 3; // movement speed
        shopItems[1, 4] = 4; // something

        //Price
        shopItems[2, 1] = _upgradePrices.DamageUpgradePrices[1];
        shopItems[2, 2] = _upgradePrices.AttackSpeedUpgradePrices[1];
        shopItems[2, 3] = _upgradePrices.MovementSpeedUpgradePrices[1];
        shopItems[2, 4] = _upgradePrices.DamageUpgradePrices[1];//This one is prob will be unused

        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;

    }

    void Update()
    {

    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int referencedItemId = ButtonRef.GetComponent<ButtonInfo>().itemID;

        if (souls >= shopItems[2, referencedItemId])
        {
            souls -= shopItems[2, referencedItemId];

            shopItems[3, referencedItemId]++;

            //Update amount of souls left
            SoulsTxt.SetText(souls.ToString());
            //Update purchased quantity text
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, referencedItemId].ToString();

            //Set a new price
            if (referencedItemId == 1)
                shopItems[2, referencedItemId] = _upgradePrices.DamageUpgradePrices[shopItems[3, referencedItemId]];
            else if (referencedItemId == 2)
                shopItems[2, referencedItemId] = _upgradePrices.AttackSpeedUpgradePrices[shopItems[3, referencedItemId]];
            else if (referencedItemId == 3)
                shopItems[2, referencedItemId] = _upgradePrices.MovementSpeedUpgradePrices[shopItems[3, referencedItemId]];
            else if (referencedItemId == 4)
                shopItems[2, referencedItemId] = _upgradePrices.MovementSpeedUpgradePrices[shopItems[3, referencedItemId]];

        }
    }

    public int GetItemQuantity(int itemId)
    {
        int quantity = shopItems[3, itemId];
        return quantity;
    }
    public int GetItemPrice(int itemId)
    {
        int quantity = shopItems[2, itemId];
        return quantity;
    }
    public void SetNewItemPrice(int itemId, int newPrice)
    {
        shopItems[2, itemId] = newPrice;
    }
}
