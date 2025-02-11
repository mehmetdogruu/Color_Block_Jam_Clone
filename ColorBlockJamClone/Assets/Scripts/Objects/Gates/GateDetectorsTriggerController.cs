using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateDetectorsTriggerController : MonoBehaviour
{
    [SerializeField] private GateDetectorController gateDetectorController;
    public bool isColTriggered=false;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("BlockDetector"))
        {
            isColTriggered = true;
            Debug.Log("BlockDetector ile çarpýþma algýlandý: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BlockDetector"))
        {
            isColTriggered = false;
            Debug.Log("BlockDetector çarpýþmasýndan çýkýldý: " + other.name);
        }
    }

}
