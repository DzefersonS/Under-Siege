using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private UpgradesSO _upgradePrices;
    [SerializeField] private CultistManager _cultistManager;

    public TMP_Text SoulsTxt;
    public int[,] shopItems = new int[5, 5];
    public int souls;


    void Start()
    {
        _cultistManager = GameObject.Find("CultistManager").GetComponent<CultistManager>();

        SoulsTxt.text = souls.ToString();

        //Item ID's
        shopItems[1, 1] = 1; //dmg
        shopItems[1, 2] = 2; //attack speed
        shopItems[1, 3] = 3; // movement speed
        shopItems[1, 4] = 4; // Cultist

        //Price
        shopItems[2, 1] = _upgradePrices.PlayerUpgradePrices[1];//Dmg
        shopItems[2, 2] = _upgradePrices.PlayerUpgradePrices[1];//atk speed
        shopItems[2, 3] = _upgradePrices.PlayerUpgradePrices[1];// movmenet speed
        shopItems[2, 4] = _upgradePrices.CultistPrices[1];      //cultist

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

            //Set a new price for Player upgrades
            if (referencedItemId == 1)
                shopItems[2, referencedItemId] = _upgradePrices.PlayerUpgradePrices[shopItems[3, referencedItemId]];
            else if (referencedItemId == 2)
                shopItems[2, referencedItemId] = _upgradePrices.PlayerUpgradePrices[shopItems[3, referencedItemId]];
            else if (referencedItemId == 3)
                shopItems[2, referencedItemId] = _upgradePrices.PlayerUpgradePrices[shopItems[3, referencedItemId]];
            //Set a new price for cultist(worker)
            else if (referencedItemId == 4)
            {
                shopItems[2, referencedItemId] = _upgradePrices.CultistPrices[shopItems[3, referencedItemId]];
                SpawnCultist();
            }
        }
    }

    public int GetItemQuantity(int itemId)
    {
        int quantity = shopItems[3, itemId];
        return quantity;
    }

    public void SpawnCultist()
    {
        _cultistManager.SpawnCultist();
    }

    public void AddSouls(int amount)
    {
        souls += amount;
        SoulsTxt.text = souls.ToString();

    }
    public int GetSouls()
    {
        return souls;
    }


}
