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
            Debug.Log("BlockDetector ile �arp��ma alg�land�: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BlockDetector"))
        {
            isColTriggered = false;
            Debug.Log("BlockDetector �arp��mas�ndan ��k�ld�: " + other.name);
        }
    }

}
