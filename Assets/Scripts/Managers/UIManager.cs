using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _HUDCanvasGO;
    [SerializeField] private GameObject _shrineCanvasGO;
    [SerializeField] private GameObject _altarCanvasGO;

    private TMP_Text _soulsCount;

    [SerializeField] private GameObjectEventSO _gameObjectEventSO;


    private void Awake()
    {
        _gameObjectEventSO.Register(EnableShopDisplay);
    }
    private void OnDestroy()
    {
        _gameObjectEventSO.Unregister(EnableShopDisplay);
    }

    void Start()
    {
        _HUDCanvasGO.SetActive(true);
        _shrineCanvasGO.SetActive(false);
        _altarCanvasGO.SetActive(false);

        _soulsCount = _HUDCanvasGO.transform.Find("SoulsTmp").gameObject.GetComponent<TMP_Text>();
    }

    public void UpdateSoulsText(int amount)
    {
        _soulsCount.text = amount.ToString();
    }


    /*Receives a GameObject from Altar or Shrine */
    /*Checks the game objects name */
    /*Disables if canvas was enabled */
    /*Enables if canvas was disabled */
    public void EnableShopDisplay()
    {
        GameObject buildingGO = _gameObjectEventSO.value;
        if (buildingGO.name == "Shrine")
        {
            if (!_shrineCanvasGO.activeSelf)
                EnableShrineCanvas();
            else
                DisableShrineCanvas();
        }
        if (buildingGO.name == "Altar")
        {
            if (!_altarCanvasGO.activeSelf)
                EnableAltarCanvas();
            else
                DisableAltarCanvas();
        }
    }

    public void DisableShopDisplay()
    {
        GameObject buildingGO = _gameObjectEventSO.value;
        if (buildingGO.name == "Shrine")
            DisableShrineCanvas();
        if (buildingGO.name == "Altar")
            DisableAltarCanvas();
    }



    public void EnableShrineCanvas()
    {
        _shrineCanvasGO.SetActive(true);
    }
    public void DisableShrineCanvas()
    {
        _shrineCanvasGO.SetActive(false);
    }
    public void EnableAltarCanvas()
    {
        _altarCanvasGO.SetActive(true);
    }

    public void DisableAltarCanvas()
    {
        _altarCanvasGO.SetActive(false);
    }

}
