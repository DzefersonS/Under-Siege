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
    [SerializeField] private GameObject _stateCanvasGO;

    [SerializeField] private TMP_Text _stateMessageText;
    [SerializeField] private TMP_Text _soulsCount;


    [SerializeField] private GameObjectEventSO _gameObjectEventSO;
    [SerializeField] private GameWonEventSO _gameWonEventSO;



    private void Awake()
    {
        _gameObjectEventSO.Register(EnableShopDisplay);
        _gameWonEventSO.Register(EnableGameStateDisplay);
    }
    private void OnDestroy()
    {
        _gameObjectEventSO.Unregister(EnableShopDisplay);
        _gameWonEventSO.Unregister(EnableGameStateDisplay);

    }

    void Start()
    {
        _HUDCanvasGO.SetActive(true);
        _shrineCanvasGO.SetActive(false);
        _altarCanvasGO.SetActive(false);

        if (_soulsCount == null)
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

    public void EnableGameStateDisplay()
    {
        bool IsWon = _gameWonEventSO.value;
        _stateCanvasGO.SetActive(true);

        if (IsWon)
            _stateMessageText.text = "Victory!";
        if (!IsWon)
            _stateMessageText.text = "Defeat!";
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
