using System;
using System.Collections;
using System.Collections.Generic;
using TeamSystem;
using UnityEngine;

public class FrameController : MonoBehaviour,IHaveTeam
{
    [SerializeField] private Team team;
    public Collider detectorCollider;


    public Team Team => team;

    public event Action<Team> OnTeamChanged;

    public void AssignTeam(Team team)
    {
        this.team = team;
        OnTeamChanged?.Invoke(team);
    }
    private void Start()
    {
        AssignTeam(team);
    }
    public bool CheckTeam(BlockController block)
    {
        if (block.Team == team)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
