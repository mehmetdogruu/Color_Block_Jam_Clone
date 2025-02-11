using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance; 
    public List<BlockController> blocks = new List<BlockController>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {

        AssignBlocksTeam();
    }
    private void AssignBlocksTeam()
    {
        foreach (var item in blocks)
        {
            item.AssignTeam(item.Team);
        }
    }
}
