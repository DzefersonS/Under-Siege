using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject m_SettingsCanvas;
    [SerializeField] private GameObject m_HUDCanvas;
    [SerializeField] private Toggle m_SoundToggle;

    private bool isPaused = false;
    private AudioSource[] m_AudioSources;

    private void Start()
    {
        m_SettingsCanvas.SetActive(false);
        m_AudioSources = FindObjectsOfType<AudioSource>();
        m_SoundToggle.onValueChanged.AddListener(ToggleSound);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        isPaused = !isPaused;
        m_HUDCanvas.SetActive(!isPaused);
        m_SettingsCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void ToggleSound(bool isOn)
    {
        foreach (var source in m_AudioSources)
        {
            source.mute = !isOn;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        m_HUDCanvas.SetActive(!true);
        m_SettingsCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}