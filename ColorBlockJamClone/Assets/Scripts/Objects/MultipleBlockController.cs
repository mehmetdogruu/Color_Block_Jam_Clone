using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MultipleBlockController : MonoBehaviour
{
    [SerializeField] private GameObject insideObjectCol;
    [SerializeField] private GameObject insideObjectDetectorCol;
    [SerializeField] private GameObject outsideObjectCol;
    [SerializeField] private BlockController outsideObjectBlockController;
    [SerializeField] private BlockController insideObjectBlockController;

    private void Start()
    {
        insideObjectCol.SetActive(false);
        insideObjectDetectorCol.SetActive(false);
        insideObjectBlockController.SetDraggable = false;
    }
    public void ShowInsideObject()
    {
        insideObjectBlockController.transform.SetParent(null);
        insideObjectCol.SetActive(true);
        outsideObjectCol.SetActive(false);
        insideObjectDetectorCol.SetActive(true);
        insideObjectBlockController.SetDraggable = true;
        Vector3 closestSnapPosition = GridManager.Instance.GetClosestGridPoint(insideObjectBlockController.transform.position);
        insideObjectBlockController.transform.position = new Vector3(closestSnapPosition.x, insideObjectBlockController.transform.position.y, closestSnapPosition.z);
        insideObjectBlockController.transform.DOScale(1f, .5f).SetDelay(.5f).OnComplete(() =>
        {
            outsideObjectCol.transform.gameObject.SetActive(false);
        });

    }
}
