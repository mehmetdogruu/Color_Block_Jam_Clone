using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamSystem;
using System;

public class BlockController : DraggableObject,IHaveTeam
{
    [SerializeField] private Team team;
    [SerializeField] private GameObject mainColliderObject;


    
    public Team Team => team;
    public GameObject Col => mainColliderObject;


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

  

}
