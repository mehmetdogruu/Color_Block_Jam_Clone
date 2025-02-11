using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/TeamSystem/Team", order = -399)]

public class Team : ScriptableObject
{
    public Color MainColor => mainColor;
    [SerializeField] private Color mainColor;




}
