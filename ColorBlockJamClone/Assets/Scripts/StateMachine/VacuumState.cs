using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumState : IState
{
    private GateDetectorController gate;
    public VacuumState(GateDetectorController gate)
    {
        this.gate = gate;
    }
    public void EnterState()
    {
        Debug.Log("VacuumState giriþ");
        gate.VacuumObject();
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }

}
