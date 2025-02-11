using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckState : IState
{
    private GateDetectorController gate;
    public CheckState(GateDetectorController gate)
    {
        this.gate = gate;
    }
    public void EnterState()
    {
        Debug.Log("CheckState giriþ");
        gate.CheckLevelCompleted();
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        Debug.Log("CheckState update");

        if (gate.CheckObjectForVacuum())
        {
            gate.ChangeState(gate.VacuumState);
        }  
    }

}
