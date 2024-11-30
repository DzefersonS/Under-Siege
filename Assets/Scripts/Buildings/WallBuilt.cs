using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilt : MonoBehaviour, IAttackable
{
    [SerializeField] private int health;
    private int tempHealth;

    [SerializeField] private GameObject _unbuiltWallGO;

    private void Awake()
    {
        _unbuiltWallGO.SetActive(false); //Just to make sure
        tempHealth = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            health = tempHealth;
            _unbuiltWallGO.SetActive(true);
            gameObject.SetActive(false);
        }

    }

}