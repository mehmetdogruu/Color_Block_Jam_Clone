using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamSystem
{
    public class TeamMaterialGroup 
    {
        public enum Type
        {
            MainColor,
        }

        public Material mainMat;

        public TeamMaterialGroup(Team team, params Material[] materials)
        {
            mainMat = new Material(materials[0]) { color = team.MainColor };

        }

        public Material GetMaterialType(Type type)
        {
            return type switch
            {
                Type.MainColor => mainMat,

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }

    }
}

