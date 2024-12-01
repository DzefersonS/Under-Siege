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

    private readonly Vector2 SOULS_UI_POSITION = new Vector2(-95f, -111f); 
    private readonly Vector3 SHRINE_POSITION = new Vector3(1.92f, 1.67f, 0f); 

    private Canvas m_TutorialCanvas;
    private int m_CurrentTextIndex = 0;
    private bool m_WaitingForShrineUpgrade = false;

    private readonly string[] m_TutorialTexts = new string[]
    {
        "Welcome, dark lord, to the tutorial of your ascension. Click \"Skip tutorial\" to skip tutorial and start the game. Click \"Next\" to continue.",
        "Hark, fallen spawn of darkness, thou hast awakened in this mortal realm with a sacred duty: to restore thy hallowed shrine to its former glory and bend all kingdoms to thy infernal will.",
        "Hearken, for thy path to dominion lies in harvesting the very souls of thine enemies, aided by the devoted whispers and dark rituals of thy faithful cultists.",
        "On the top left corner of the screen, thou shalt find the number of souls thou hast amassed. Souls are the currency of thy power, and thou must gather them to empower thy dark designs.",
        "Take heed, dark one - to unlock other upgrades, you must first enhance your shrine's power. Make your way to the shrine and strengthen it."
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
    }

    private void Update()
    {
        if (m_WaitingForShrineUpgrade && m_ShopManager != null && m_ShopManager.shrineLevel > 0)
        {
            m_WaitingForShrineUpgrade = false;
            m_NextButton.interactable = true;
            m_UIArrow.gameObject.SetActive(false);
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
            
            if (m_CurrentTextIndex == 3)
            {
                m_UIArrow.gameObject.SetActive(true);
                m_WorldArrow.gameObject.SetActive(false);
                PositionUIArrow(SOULS_UI_POSITION);
            }
            else if (m_CurrentTextIndex == 4)
            {
                m_UIArrow.gameObject.SetActive(false);
                m_WorldArrow.gameObject.SetActive(true);
                PositionWorldArrow(SHRINE_POSITION);
                
                m_WaitingForShrineUpgrade = true;
                m_NextButton.gameObject.SetActive(false);
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

    private void FinishTutorial()
    {
        if (m_WaveManager != null)
        {
            m_WaveManager.EnableWaveSpawning();
        }
    }
}