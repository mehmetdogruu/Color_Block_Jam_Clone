using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class BoosterManager : MonoBehaviour
{
    private bool isWaitingForClick = false;
    private Team selectedTeam;
    [SerializeField] private Button boosterButton; 
    [SerializeField] private TMP_Text boosterMessage; 
    [SerializeField] private ParticleSystem tornadoVFX; 

    public void ActivateClickMode()
    {
        if (!isWaitingForClick)
        {
            StartCoroutine(WaitForObjectClick());
        }
    }

    private IEnumerator WaitForObjectClick()
    {
        isWaitingForClick = true;
        Debug.Log("Bir objeye týklanmasý bekleniyor...");
        boosterButton.transform.DOScale(1.3f, .5f);
        boosterMessage.transform.DOScale(1f, .5f);
        

        while (isWaitingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    BlockController blockController = clickedObject.GetComponentInParent<BlockController>();

                    if (blockController != null)
                    {
                        Debug.Log("Týklanan obje: " + clickedObject.name);
                        Debug.Log("Team deðeri: " + blockController.Team);
                        selectedTeam = blockController.Team;
                        boosterButton.transform.DOPunchRotation(Vector3.one * 2f, .5f);
                        tornadoVFX.Play();
                        foreach (var item in BlockManager.Instance.blocks)
                        {
                            if (item.Team==selectedTeam)
                            {

                                item.transform.DOPunchScale(Vector3.one * .5f, .5f).OnComplete(() =>
                                {
                                    item.transform.DOScale(0f, .5f).OnComplete(() =>
                                    {
                                        item.gameObject.SetActive(false);
                                        BlockManager.Instance.blocks.Remove(item);


                                    });
                                });
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Týklanan obje BlockController içermiyor. Ýþlem iptal edildi.");
                    }

                    isWaitingForClick = false;
                    boosterButton.transform.DOScale(1f, .5f);
                    boosterMessage.transform.DOScale(0f, .5f);


                }
            }
            yield return null;
        }
    }
}
