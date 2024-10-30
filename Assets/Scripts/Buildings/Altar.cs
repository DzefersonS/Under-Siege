using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private bool isAccessible; // Currently does nothing, will do something when waves become a thing
    [SerializeField] private GameObjectEventSO _gameObjectEventSO;


    void Start()
    {
        isAccessible = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isAccessible)
        {
            _gameObjectEventSO.value = this.gameObject;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _gameObjectEventSO.value = this.gameObject;

        }
    }
}
