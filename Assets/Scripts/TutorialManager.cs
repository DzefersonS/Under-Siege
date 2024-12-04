using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private WaveManager m_WaveManager;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private Button m_NextButton;
    [SerializeField] private GameObject m_UIArrow;
    [SerializeField] private GameObject m_WorldArrow;
    [SerializeField] private ShopManager m_ShopManager;
    [SerializeField] private InputBasedMovement m_PlayerMovement;
    [SerializeField] private PlayerInputsSO m_PlayerInputs;
    [SerializeField] private GameObject m_PowerupCanvas;

    private readonly Vector2 SOULS_UI_POSITION = new Vector2(-66f, -111f); 
    private readonly Vector2 HEALTH_UI_POSITION = new Vector2(-411f, -111f); 
    private readonly Vector3 SHRINE_POSITION = new Vector3(1.92f, 1.67f, 0f); 
    private readonly Vector3 ALTAR_POSITION = new Vector3(13.13f, 2.34f, 0f); 

    private Canvas m_TutorialCanvas;
    private int m_CurrentTextIndex = 0;
    private bool m_WaitingForShrineUpgrade = false;
    private bool m_WaitingForCultists = false;
    private float m_OriginalAcceleration = 20f;

    private bool m_HasPressedW = false;
    private bool m_HasPressedA = false;
    private bool m_HasPressedD = false;
    private bool m_HasPressedSpace = false;
    private bool m_CheckingMovementKeys = false;

    private readonly string[] m_TutorialTexts = new string[]
    {
        "Welcome, dark lord, to the tutorial of your ascension. Click \"Skip tutorial\" to skip tutorial and start the game. Click \"Next\" to continue.",
        "Hark, fallen spawn of darkness, thou hast awakened in this mortal realm with a sacred duty: to restore thy hallowed shrine to its former glory and bend all kingdoms to thy infernal will.",
        "Press the W, A, and D keys to guide your movement through the realm. Press SPACE to unleash thy dark powers",
        "On the top left corner of the screen, thou shalt find the number of souls thou hast amassed. Souls are the currency of thy power, and thou must gather them to empower thy dark designs.",
        "Take heed, dark one - to unlock other upgrades, you must first enhance your shrine's power. Make your way to the shrine and strengthen it.",
        "Hearken, for thy path to dominion lies in harvesting the souls of thine fallen enemies, aided by the devoted whispers and dark rituals of thy faithful cultists.",
        "Thy faithful cultists shall harvest souls in thy name - venture forth to the hallowed altar and summon two of these devoted servants to begin thy dark harvest.",
        "BEWARE, dark one - righteous warriors approach to lay waste to thy sacred shrine. Protect it at all costs, for its destruction shall spell thy doom. The shrine's health bar is visible at the top of your screen.",
    };

    private void Awake()
    {
        m_TutorialCanvas = GetComponent<Canvas>();
        
        if (m_TutorialText != null && m_TutorialTexts.Length > 0)
        {
            m_TutorialText.text = m_TutorialTexts[0];
        }

        if (m_UIArrow != null)
        {
            m_UIArrow.gameObject.SetActive(false);
        }
        if (m_WorldArrow != null)
        {
            m_WorldArrow.gameObject.SetActive(false);
        } 
        if (m_PlayerMovement != null)
        {
            m_PlayerMovement.EnableInput(false);
        }
        if (m_PowerupCanvas != null)
        {
            m_PowerupCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (m_WaitingForShrineUpgrade && m_ShopManager != null && m_ShopManager.shrineLevel > 0)
        {
            m_WaitingForShrineUpgrade = false;
            m_WorldArrow.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(true);
            ShowNextText();
        }

        if (m_CheckingMovementKeys)
        {
            if (Input.GetKey(m_PlayerInputs.moveLeft))
            {
                m_HasPressedA = true;
            }
            if (Input.GetKey(m_PlayerInputs.moveRight))
            {
                m_HasPressedD = true;
            }
            if (Input.GetKey(m_PlayerInputs.jump))
            {
                m_HasPressedW = true;
            }
            if (Input.GetKey(m_PlayerInputs.attack))
            {
                m_HasPressedSpace = true;
            }

            if (m_HasPressedW && m_HasPressedA && m_HasPressedD && m_HasPressedSpace)
            {
                m_CheckingMovementKeys = false;
                ShowNextText();
            }
        }

        if (m_WaitingForCultists && m_ShopManager != null && m_ShopManager.shopItems[3, 4] >= 2)
        {
            m_WaitingForCultists = false;
            m_WorldArrow.gameObject.SetActive(false);
            ShowNextText();
        }
    }

    public void ShowNextText()
    {
        if (m_WaitingForShrineUpgrade)
        {
            return;
        }

        m_CurrentTextIndex++;

        if (m_CurrentTextIndex < m_TutorialTexts.Length)
        {
            m_TutorialText.text = m_TutorialTexts[m_CurrentTextIndex];
            if (m_CurrentTextIndex == 2)
            {
                m_PlayerMovement.EnableInput(true);
                m_CheckingMovementKeys = true;
                m_NextButton.gameObject.SetActive(false);
                
                m_HasPressedW = false;
                m_HasPressedA = false;
                m_HasPressedD = false;
            }
            else if (m_CurrentTextIndex == 3)
            {
                m_PlayerMovement.EnableInput(false);
                m_UIArrow.gameObject.SetActive(true);
                m_WorldArrow.gameObject.SetActive(false);
                m_NextButton.gameObject.SetActive(true);
                PositionUIArrow(SOULS_UI_POSITION);
            }
            else if (m_CurrentTextIndex == 4)
            {
                m_PlayerMovement.EnableInput(true);
                m_UIArrow.gameObject.SetActive(false);
                m_WorldArrow.gameObject.SetActive(true);
                PositionWorldArrow(SHRINE_POSITION);
                m_WaitingForShrineUpgrade = true;
                m_NextButton.gameObject.SetActive(false);
            }
            else if (m_CurrentTextIndex == 6)
            {
                m_WorldArrow.gameObject.SetActive(true);
                PositionWorldArrow(ALTAR_POSITION);
                m_WaitingForCultists = true;
                m_NextButton.gameObject.SetActive(false);
            }
            else if (m_CurrentTextIndex == 7)
            {
                m_UIArrow.gameObject.SetActive(true);
                m_WorldArrow.gameObject.SetActive(false);
                PositionUIArrow(HEALTH_UI_POSITION);
                m_NextButton.gameObject.SetActive(true);
            }
            else
            {
                m_UIArrow.gameObject.SetActive(false);
                m_WorldArrow.gameObject.SetActive(false);
            }
        }
        else
        {
            m_NextButton.gameObject.SetActive(false);
            FinishTutorial();
        }
    }

    public void PositionWorldArrow(Vector3 position)
    {
        if (m_WorldArrow != null)
        {
            m_WorldArrow.transform.position = position;
        }
    }

    public void PositionUIArrow(Vector2 position)
    {
        if (m_UIArrow != null)
        {
            RectTransform rectTransform = m_UIArrow.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = position;
            }
        }
    }

    public void FinishTutorial()
    {
        m_TutorialCanvas.gameObject.SetActive(false);
        if (m_WaveManager != null)
        {
            m_WaveManager.EnableWaveSpawning();
        }
        if (m_PlayerMovement != null)
        {
            m_PlayerMovement.EnableInput(true);
        }
        if (m_PowerupCanvas != null)
        {
            m_PowerupCanvas.SetActive(true);
        }
    }
}