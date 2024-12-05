using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsExit : MonoBehaviour
{
    public void OnExitButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
