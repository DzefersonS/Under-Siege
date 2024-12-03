using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameWonEventSO _gameWonEventSO;

    private void Awake()
    {
        _gameWonEventSO.Register(CheckWinCondition);
    }

    private void OnDestroy()
    {
        _gameWonEventSO.Unregister(CheckWinCondition);
    }

    private void CheckWinCondition()
    {
        bool gameWon = _gameWonEventSO.value;
        Time.timeScale = 0;

        //In an if for sounds effects to be added later
        if (gameWon)
        {
            StartCoroutine(ReturnToMenuAfterDelay(3));
        }
        else
        {
            StartCoroutine(ReturnToMenuAfterDelay(3));
        }

    }

    private IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Wait for delay in real time
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }


}
