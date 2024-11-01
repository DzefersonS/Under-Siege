using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultistBaseState
{

    public abstract void EnterState(Cultist cultist);
    public abstract void UpdateState(Cultist cultist);
    public abstract void ExitState();


}
