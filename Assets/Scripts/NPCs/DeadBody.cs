using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    public bool isTaken = false;
    void Start()
    {
        isTaken = false;
    }


    public bool GetIsTaken()
    {
        return isTaken;
    }

}
