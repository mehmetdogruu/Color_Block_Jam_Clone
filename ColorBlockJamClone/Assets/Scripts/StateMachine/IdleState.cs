using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private GateDetectorController gate;
    public IdleState(GateDetectorController gate)
    {
        this.gate = gate;
    }
    public void EnterState()
    {
        Debug.Log("IdleState giriþ");


    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }

}
