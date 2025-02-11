using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamSystem
{
    public class SharedMaterialManager : MonoBehaviour
    {
        public static SharedMaterialManager Instance;

        [SerializeField] private Material mainMaterial;

        private Dictionary<Team, TeamMaterialGroup> _teamMaterials = new();

        private void Awake()
        {
            Instance = this;
        }

        public Material GetTeamMaterial(Team team, TeamMaterialGroup.Type type)
        {
            if (team == null)
            {
                return GetDefaultMaterialByType(type);
            }

            var teamRegistered = _teamMaterials.TryGetValue(team, out var group);
            if (!teamRegistered) group = RegisterTeam(team);

            return group.GetMaterialType(type);
        }

        private TeamMaterialGroup RegisterTeam(Team team)
        {
            var group = new TeamMaterialGroup(team, mainMaterial);
            _teamMaterials.Add(team,group);
            return group;
        }

        private Material GetDefaultMaterialByType(TeamMaterialGroup.Type type)
        {
            return type switch
            {
                TeamMaterialGroup.Type.MainColor => mainMaterial,

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

    }
}

