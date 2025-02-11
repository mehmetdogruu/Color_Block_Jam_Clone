using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance; 

    public bool IsInitialized { get; private set; }

    public List<Vector3> gridPoints = new List<Vector3>(); 
    public GridObject[,] grids;
    public List<GridObject> gridObjects = new List<GridObject>();
    public int rows = 3; 
    public int columns = 4; 
    public float spacing = 1.0f; 
    public float cellSize = 1.0f;
    [SerializeField] private Transform gridParentTransform;
    [SerializeField] private Transform frameParentTransform;


    public event Action OnGridsCreated;


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

    [ContextMenu("Instantiate Grids")]
    public void GenerateGrid()
    {
        grids = new GridObject[rows, columns];
        gridPoints.Clear(); 
        gridObjects.Clear(); 

        var gridPrefab = Resources.Load<GameObject>("Grid");
        Vector3 prefabSize = gridPrefab.transform.localScale;
        Vector3 startPosition = new Vector3(0, 0.1f, 0);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                
                Vector3 position = new Vector3(
                    startPosition.x + col * (cellSize + spacing),
                    0.1f,
                    startPosition.z + row * (cellSize + spacing)
                );

                GameObject gridCellObject = Instantiate(gridPrefab, position, Quaternion.identity, gridParentTransform);
                GridObject gridCell = gridCellObject.GetComponent<GridObject>();
                gridObjects.Add(gridCell);
                gridCell.Initialize(row, col, this);
                grids[row, col] = gridCell;
                gridPoints.Add(position); 
            }
        }

        IsInitialized = true;
        OnGridsCreated?.Invoke();
        CenterCameraOnGrid(prefabSize);
    }

    private void CenterCameraOnGrid(Vector3 prefabSize)
    {
        float totalGridWidth = columns * (prefabSize.x + spacing) - spacing;
        float totalGridHeight = rows * (prefabSize.z + spacing) - spacing;

        int largerGridDimension = Mathf.Max(rows, columns);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = largerGridDimension * 0.8f+3;

        Vector3 centerPosition = new Vector3(
            gridParentTransform.position.x + totalGridWidth / 2f - prefabSize.x / 2f,
            10f,  // Yüksekliði sabit tut
            gridParentTransform.position.z + totalGridHeight / 2f - prefabSize.z / 2f
        );

        Camera.main.transform.position = new Vector3(centerPosition.x, 10f, centerPosition.z);
        Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    [ContextMenu("Instantiate Grid Frame")]
    public void InstantiateGridFrame()
    {
        var framePrefab = Resources.Load<GameObject>("FramePrefab");

        // Üst ve alt çerçeveler
        for (int col = 1; col <= columns; col++)
        {
            // Üst çerçeve
            Vector3 topPosition = new Vector3(
                frameParentTransform.position.x + (col-1) * (cellSize + spacing),
                0.1f,
                frameParentTransform.position.z + (rows-1) * cellSize +.9f
            );
            Instantiate(framePrefab, topPosition, Quaternion.Euler(0, -180, 0), frameParentTransform);

            // Alt çerçeve
            Vector3 bottomPosition = new Vector3(
                frameParentTransform.position.x + (col-1) * (cellSize + spacing),
                0.1f,
                frameParentTransform.position.z - cellSize / 2f -.3f
            );
            Instantiate(framePrefab, bottomPosition, Quaternion.identity, frameParentTransform);
        }

        // Sol ve sað çerçeveler
        for (int row = 1; row <= rows; row++)
        {
            // Sol çerçeve
            Vector3 leftPosition = new Vector3(
                frameParentTransform.position.x - cellSize / 2f-.3f,
                0.1f,
                frameParentTransform.position.z + (row-1) * (cellSize + spacing)
            );
            Instantiate(framePrefab, leftPosition, Quaternion.Euler(0, 90f, 0), frameParentTransform);

            // Sað çerçeve
            Vector3 rightPosition = new Vector3(
                frameParentTransform.position.x + (columns-1) * cellSize + .9f, 
                0.1f,
                frameParentTransform.position.z + (row - 1) * (cellSize + spacing)
            );
            Instantiate(framePrefab, rightPosition, Quaternion.Euler(0, -90f, 0), frameParentTransform);
        }
    }
    public Vector3 GetClosestGridPoint(Vector3 position)
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (Vector3 gridPoint in gridPoints)
        {
            float distance = Vector3.Distance(position, gridPoint);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = gridPoint;
            }
        }

        return closestPoint;
    }
    public GridObject GetGridCellFromPosition(Vector3 position)
    {
        foreach (var grid in grids)
        {
            if (grid == null) continue;

            if (Vector3.Distance(grid.transform.position, position) < cellSize / 2)
            {
                return grid;
            }
        }

        return null; 
    }

}
