using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BlockMovementType
{
    DefaultBlock,    
    HorizontalBlock,
    VerticalBlock    
}

public class DraggableObject : MonoBehaviour
{
    public BlockMovementType movementType = BlockMovementType.DefaultBlock; 
    public bool dragging { get; set; } = false;

    private Vector3 initialPosition;
    private Rigidbody rb;
    private float dragSpeed = 20f;
    public bool SetDraggable = true;
    public List<GridObject> overlappingGrids = new List<GridObject>();
    [SerializeField] private GameObject horizontalArrows;
    [SerializeField] private GameObject verticalArrows;

    public event Action OnBlocksMoved;

    protected virtual void Start()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;


    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (movementType == BlockMovementType.HorizontalBlock)
        {
            horizontalArrows.SetActive(true);
        }
        else if (movementType == BlockMovementType.VerticalBlock)
        {
            verticalArrows.SetActive(true);

        }
    }

    private void OnMouseDown()
    {
        if (!SetDraggable) return;
        dragging = true;
        initialPosition = transform.position;
        SetRigidbody(false);
    }

    private void FixedUpdate()
    {
        if (dragging)
        {
            Vector3 mouseWorldPos = GetMouseWorldPos();
            Vector3 targetPosition = new Vector3(mouseWorldPos.x, initialPosition.y, mouseWorldPos.z);

            switch (movementType)
            {
                case BlockMovementType.HorizontalBlock:
                    targetPosition.z = initialPosition.z;  
                    break;
                case BlockMovementType.VerticalBlock:
                    targetPosition.x = initialPosition.x;  
                    break;
                case BlockMovementType.DefaultBlock:
                    // Her iki eksende serbest hareket
                    break;
            }

            // Pozisyonu grid sýnýrlarý arasýnda tut
            targetPosition = ClampToGridBounds(targetPosition);

            MoveTo(targetPosition);
        }
    }
    private Vector3 ClampToGridBounds(Vector3 targetPosition)
    {
        float minX = GridManager.Instance.gridPoints[0].x;
        float maxX = GridManager.Instance.gridPoints[GridManager.Instance.columns - 1].x; 

        float minZ = GridManager.Instance.gridPoints[0].z;
        float maxZ = GridManager.Instance.gridPoints[(GridManager.Instance.rows - 1) * GridManager.Instance.columns].z; 

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        return targetPosition;
    }


    private void OnMouseUp()
    {
        if (!dragging) return;
        dragging = false;

        if (IsCollidingWithFullGrid())
        {
            transform.position = initialPosition;
        }
        else
        {
            MarkGridsAsFull();  

            Vector3 closestSnapPosition = GridManager.Instance.GetClosestGridPoint(transform.position);
            transform.position = new Vector3(closestSnapPosition.x, initialPosition.y, closestSnapPosition.z);
        }
        UpdateGrids();
        SetRigidbody(true);
    }

    private bool IsCollidingWithFullGrid()
    {
        foreach (var grid in overlappingGrids)
        {
            if (grid.isFull && grid.filledByObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void MarkGridsAsFull()
    {
        foreach (var grid in overlappingGrids)
        {
            grid.isFull = true;
            grid.filledByObject = gameObject;
        }
    }

    private void MoveTo(Vector3 target)
    {
        var offset = target - rb.position;
        var direction = offset.sqrMagnitude > 1 ? offset.normalized : offset;
        Move(direction);
    }

    private void Move(Vector3 direction)
    {
        var movement = direction * dragSpeed;
        rb.velocity = movement;
    }

    private void SetRigidbody(bool condition)
    {
        rb.isKinematic = condition;

        if (condition)
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        GridObject grid = other.GetComponent<GridObject>();
        if (grid != null && !overlappingGrids.Contains(grid))
        {
            overlappingGrids.Add(grid);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GridObject grid = other.GetComponent<GridObject>();
        if (grid != null && overlappingGrids.Contains(grid))
        {
            overlappingGrids.Remove(grid);
        }
    }

    private void UpdateGrids()
    {
        OnBlocksMoved?.Invoke();

        foreach (var item in GridManager.Instance.gridObjects)
        {
            item.isFull = false;
            item.filledByObject = null;
        }
        foreach (var item in overlappingGrids)
        {
            item.filledByObject = this.gameObject;
            item.isFull = true;
        }
    }
}
