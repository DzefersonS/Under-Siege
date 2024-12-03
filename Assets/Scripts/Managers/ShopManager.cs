using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private UpgradesSO _upgradePrices;
    [SerializeField] private CultistManager _cultistManager;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private CultistEventSO _cultistDeathEventSO;
    [SerializeField] private UpgradePurchaseEventSO _upgradePurchaseEventSO;
    [SerializeField] private GameWonEventSO _gameWonEventSO;

    [SerializeField] private AudioSource m_ActionDeniedSFX;

    [SerializeField] private int maxUpgradesPerLevel = 5;

    public int[,] shopItems = new int[5, 6];
    public int shrineLevel = 0;
    public int souls;

    private void Awake()
    {
        _cultistDeathEventSO.Register(DecreaseCultistQuantity);
    }

    private void OnDestroy()
    {
        _cultistDeathEventSO.Unregister(DecreaseCultistQuantity);
    }
    void Start()
    {
        _cultistManager = GameObject.Find("CultistManager").GetComponent<CultistManager>();
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        if (_UIManager == null)
            Debug.LogWarning("In ShopManager, UI Manager is null");
        if (_cultistManager == null)
            Debug.LogWarning("In ShopManager, Cultist Manager is null");

        _UIManager.UpdateSoulsText(souls);


        //Item ID's
        shopItems[1, 1] = 1; //dmg
        shopItems[1, 2] = 2; //attack speed
        shopItems[1, 3] = 3; // movement speed
        shopItems[1, 4] = 4; // Cultist
        shopItems[1, 5] = 5; // Shrine


        //Price
        shopItems[2, 1] = _upgradePrices.PlayerUpgradePrices[0];    //Dmg
        shopItems[2, 2] = _upgradePrices.PlayerUpgradePrices[0];    //atk speed
        shopItems[2, 3] = _upgradePrices.PlayerUpgradePrices[0];    // movmenet speed
        shopItems[2, 4] = _upgradePrices.CultistPrices[0];          //cultist
        shopItems[2, 5] = _upgradePrices.ShrinePrices[0];           //Shrine


        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;

        shrineLevel = shopItems[3, 5];
    }

    //Altar
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int referencedItemId = ButtonRef.GetComponent<ButtonInfo>().itemID;

        if (IsEligibleForPurchase(referencedItemId))
        {
            souls -= shopItems[2, referencedItemId];

            shopItems[3, referencedItemId]++;

            //Update amount of souls left
            _UIManager.UpdateSoulsText(souls);

            //Update purchased quantity text
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, referencedItemId].ToString();

            //Set a new price for Player upgrades
            if (referencedItemId >= 1 && referencedItemId <= 3)
                shopItems[2, referencedItemId] = _upgradePrices.PlayerUpgradePrices[shopItems[3, referencedItemId]];
            //Set a new price for cultist(worker)
            else if (referencedItemId == 4)
            {
                shopItems[2, referencedItemId] = _upgradePrices.CultistPrices[shopItems[3, referencedItemId]];
                _cultistManager.SpawnCultist();
            }
            //Set a new price for shrine
            else if (referencedItemId == 5)
            {
                shrineLevel++;
                if (!CheckIfWin())
                    shopItems[2, referencedItemId] = _upgradePrices.ShrinePrices[shopItems[3, referencedItemId]];
            }

            //Call event for UpgradeController to Apply Upgrade
            _upgradePurchaseEventSO.value = referencedItemId;
        }
        else
        {
            m_ActionDeniedSFX.Play();
        }
    }

    private bool IsEligibleForPurchase(int itemId)
    {
        //check if enough souls
        if (souls >= shopItems[2, itemId])
        {
            //check if shrine needs upgrading
            if (maxUpgradesPerLevel * shrineLevel >= shopItems[3, itemId] + 1)
                return true;
            else if (itemId == 5)// if buying shrine upgrade
                return true;
        }
        return false;
    }

    private bool CheckIfWin()
    {
        if (shrineLevel == _upgradePrices.ShrinePrices.Length)
        {
            Debug.Log("Game Won");
            _gameWonEventSO.value = true;

            return true;
        }
        return false;
    }

    public int GetItemQuantity(int itemId)
    {
        int quantity = shopItems[3, itemId];
        return quantity;
    }

    public void AddSouls(int amount)
    {
        souls += amount;
        _UIManager.UpdateSoulsText(souls);
    }
    public int GetSouls()
    {
        return souls;
    }
    public void DecreaseCultistQuantity()
    {
        shopItems[3, 4]--;
    }
    public void DecreaseQuantity(int itemId, int amountToDecrease)
    {
        shopItems[3, itemId] -= amountToDecrease;
    }

}
