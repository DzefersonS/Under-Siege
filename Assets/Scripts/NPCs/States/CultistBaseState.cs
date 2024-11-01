using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultistBaseState : MonoBehaviour
{
    protected Cultist cultist;

    // set Cultist reference
    public void SetCultist(Cultist cultistInstance)
    {
        cultist = cultistInstance;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();


}
