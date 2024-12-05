using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject _unbuiltWallGO;
    [SerializeField] private GameObject _soulGO;
    [SerializeField] private GameObject _builtWallGO;
    [SerializeField] private PlayerInputsSO m_PlayerInputsSO;

    [SerializeField] private ShopManager _shopManager;

    private bool isWIthinWall;

    private void Update()
    {
        if (isWIthinWall && Input.GetKey(m_PlayerInputsSO.buy))
            BuildWall();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            isWIthinWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isWIthinWall = false;
        }
    }


    private void BuildWall()
    {
        if (_unbuiltWallGO.activeInHierarchy)
        {
            _unbuiltWallGO.SetActive(false);
            _soulGO.SetActive(false);
            _builtWallGO.SetActive(true);
            _shopManager.AddSouls(-1);
        }
    }

}
