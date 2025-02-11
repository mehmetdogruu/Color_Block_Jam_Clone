using UnityEngine;

public class DragManager : MonoBehaviour
{
    private Camera mainCamera;
    private IDraggable currentDraggable;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButton(0) && currentDraggable != null)
        {
            currentDraggable.HoldClick();
        }
        else if (Input.GetMouseButtonUp(0) && currentDraggable != null)
        {
            currentDraggable.UpClick();
            currentDraggable = null; // Drag iþlemi tamamlandý
        }
    }

    private void HandleMouseDown()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var draggable = hit.collider.GetComponentInParent<IDraggable>();
            if (draggable != null)
            {
                currentDraggable = draggable;
                currentDraggable.DownClick();
            }
        }
    }
}
