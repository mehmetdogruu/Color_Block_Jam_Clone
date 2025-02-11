using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance; // Singleton instance

    public Team[] Teams; 
    public Dictionary<Team, int> teamDictionary = new Dictionary<Team, int>();

    public int redTeamCount;
    public int blueTeamCount;
    public int greenTeamCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Çift instance varsa yok et
        }
    }

    public Team GetRandomTeam()
    {
        var team = Teams[Random.Range(0, Teams.Length)];

        if (!teamDictionary.ContainsKey(team))
        {
            teamDictionary[team] = 1;
        }
        else
        {
            teamDictionary[team]++;
        }

        return team;
    }


    public List<Team> GetAllTeamsInDictionary()
    {
        return new List<Team>(teamDictionary.Keys);
    }

    public int GetTeamCount(Team team)
    {
        if (teamDictionary.TryGetValue(team, out int count))
        {
            return count;
        }

        return 0;
    }

    public void DecreaseTeamCount(Team team, int amount)
    {
        if (teamDictionary.TryGetValue(team, out int currentCount))
        {
            int newCount = Mathf.Max(0, currentCount - amount); 
            teamDictionary[team] = newCount;
            Debug.Log($"Team {team.name} count decreased to {newCount}");
        }
        else
        {
            Debug.LogWarning($"Team {team.name} not found in dictionary.");
        }
    }



}
