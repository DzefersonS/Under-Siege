using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Shrine _Shrine;
    [SerializeField] private GameObject _HUDCanvasGO;
    [SerializeField] private GameObject _shrineCanvasGO;
    [SerializeField] private GameObject _altarCanvasGO;
    [SerializeField] private GameObject _stateCanvasGO;
    [SerializeField] private GameObject _tutorialCanvasGO;
    [SerializeField] private GameObject _skipButton;
    [SerializeField] private TMP_Text _stateMessageText;
    [SerializeField] private TMP_Text _soulsCount;

    [SerializeField] private GameWonEventSO _gameWonEventSO;

    private bool _canOpenShrineCanvas = true;
    private bool _canOpenAltarCanvas = true;



    private void Awake()
    {
        _gameWonEventSO.Register(EnableGameStateDisplay);
    }
    private void OnDestroy()
    {
        _gameWonEventSO.Unregister(EnableGameStateDisplay);
    }

    void Start()
    {
        _HUDCanvasGO.SetActive(true);
        _shrineCanvasGO.SetActive(false);
        _altarCanvasGO.SetActive(false);
        _stateCanvasGO.SetActive(false);

        if (_soulsCount == null)
            _soulsCount = _HUDCanvasGO.transform.Find("SoulsTmp").gameObject.GetComponent<TMP_Text>();
    }

    public void UpdateSoulsText(int amount)
    {
        _soulsCount.text = amount.ToString();
    }

    public void SetShrineCanvasAccess(bool canAccess)
    {
        _canOpenShrineCanvas = canAccess;
        if (canAccess)
        {
            _Shrine.CheckIfPlayerInHitbox();
        }
    }

    public void SetAltarCanvasAccess(bool canAccess)
    {
        _canOpenAltarCanvas = canAccess;
    }

    public void EnableGameStateDisplay()
    {
        bool IsWon = _gameWonEventSO.value;
        _stateCanvasGO.SetActive(true);
        _HUDCanvasGO.SetActive(false);
        _shrineCanvasGO.SetActive(false);
        _altarCanvasGO.SetActive(false);

        if (IsWon)
            _stateMessageText.text = "Victory!";
        if (!IsWon)
            _stateMessageText.text = "Defeat!";
    }

    private void ToggleTutorialButtons(bool show)
    {
        if (_tutorialCanvasGO.activeSelf)
        {
            _skipButton.SetActive(show);
        }
    }

    public void EnableShrineCanvas()
    {
        if (_canOpenShrineCanvas)
        {
            _shrineCanvasGO.SetActive(true);
            ToggleTutorialButtons(false);
        }
    }
    public void DisableShrineCanvas()
    {
        _shrineCanvasGO.SetActive(false);
        ToggleTutorialButtons(true);
    }
    public void EnableAltarCanvas()
    {
        if (_canOpenAltarCanvas)
        {
            _altarCanvasGO.SetActive(true);
            ToggleTutorialButtons(false);
        }
    }

    public void DisableAltarCanvas()
    {
        _altarCanvasGO.SetActive(false);
        ToggleTutorialButtons(true);
    }

}
