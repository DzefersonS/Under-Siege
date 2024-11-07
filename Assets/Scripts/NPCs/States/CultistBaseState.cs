using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultistBaseState : MonoBehaviour
{
    protected Cultist cultist;


    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    // set Cultist reference for each state
    public void SetCultist(Cultist cultistInstance)
    {
        cultist = cultistInstance;
    }
}
