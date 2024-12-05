using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using static ShopManager.ECategories;
using static ShopManager.EUpgradeID;

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

    public enum ECategories
    {
        ID = 1,
        Price = 2,
        Quantity = 3
    }

    public enum EUpgradeID
    {
        Damage = 1,
        AttackSpeed = 2,
        MovementSpeed = 3,
        Cultist = 4,
        Shrine = 5
    }

    public int[,] shopItems = new int[5, 6];
    public int shrineLevel = 0;
    public int souls;

    public bool m_OnlyCultistsAllowed = false;

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
        shopItems[(int)Price, (int)Damage] = _upgradePrices.PlayerUpgradePrices[0];    //Dmg
        shopItems[(int)Price, (int)AttackSpeed] = _upgradePrices.PlayerUpgradePrices[0];    //atk speed
        shopItems[(int)Price, (int)MovementSpeed] = _upgradePrices.PlayerUpgradePrices[0];    // movmenet speed
        shopItems[(int)Price, (int)EUpgradeID.Cultist] = _upgradePrices.CultistPrices[0];          //cultist
        shopItems[(int)Price, (int)EUpgradeID.Shrine] = _upgradePrices.ShrinePrices[0];           //Shrine


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
            souls -= shopItems[(int)Price, referencedItemId];

            shopItems[(int)Quantity, referencedItemId]++;

            //Update amount of souls left
            _UIManager.UpdateSoulsText(souls);

            //Update purchased quantity text
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, referencedItemId].ToString();

            //Set a new price for Player upgrades
            if (referencedItemId >= (int)Damage && referencedItemId <= (int)MovementSpeed)
            {
                shopItems[(int)Price, referencedItemId] = _upgradePrices.PlayerUpgradePrices[shopItems[3, referencedItemId]];
            }
            //Set a new price for cultist(worker)
            else if (referencedItemId == (int)EUpgradeID.Cultist)
            {
                shopItems[(int)Price, referencedItemId] = _upgradePrices.CultistPrices[shopItems[3, referencedItemId]];
                _cultistManager.SpawnCultist();
            }
            //Set a new price for shrine
            else if (referencedItemId == (int)EUpgradeID.Shrine)
            {
                shrineLevel++;
                if (!CheckIfWin())
                    shopItems[(int)Price, referencedItemId] = _upgradePrices.ShrinePrices[shopItems[3, referencedItemId]];
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
        if (m_OnlyCultistsAllowed && itemId != 4)
        {
            return false;
        }

        //check if enough souls
        if (souls >= shopItems[(int)Price, itemId])
        {
            //check if shrine needs upgrading
            if (maxUpgradesPerLevel * shrineLevel >= shopItems[3, itemId] + 1 && itemId > 0 && itemId < 5)
                return true;
            else if (itemId == (int)EUpgradeID.Shrine && (shrineLevel + 1 <= _upgradePrices.ShrinePrices.Length))// if buying shrine upgrade
                return true;
        }
        return false;
    }

    private bool CheckIfWin()
    {
        if (shrineLevel == _upgradePrices.ShrinePrices.Length)
        {
            _gameWonEventSO.value = true;

            return true;
        }
        return false;
    }

    public int GetItemQuantity(int itemId)
    {
        int quantity = shopItems[(int)Quantity, itemId];
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
        shopItems[(int)Quantity, (int)EUpgradeID.Cultist]--;
        shopItems[(int)Price, (int)EUpgradeID.Cultist] = _upgradePrices.CultistPrices[shopItems[3, (int)EUpgradeID.Cultist]];

    }
    public void DecreaseQuantity(int itemId, int amountToDecrease)
    {
        shopItems[(int)Quantity, itemId] -= amountToDecrease;
    }

}
