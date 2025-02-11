using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public int row;
    public int column;
    public bool isFull;
    public GameObject filledByObject ;  // Grid'i dolduran objeyi takip eder


    private GridManager gridManager;


    public void Initialize(int row, int column, GridManager gridManager)
    {
        this.row = row;
        this.column = column;
        this.gridManager = gridManager;
    }
  


}

