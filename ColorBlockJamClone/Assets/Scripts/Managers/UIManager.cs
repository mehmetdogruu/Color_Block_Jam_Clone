using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private TMP_Text coinText;

    [SerializeField] private GameObject coinPrefab; 
    [SerializeField] private RectTransform startPoint;  
    [SerializeField] private RectTransform targetButton;  
    [SerializeField] private RectTransform coinParentTransform;  
    private int coinCount = 10; 
    private float duration = 1.5f;  
    private Vector2 spreadRange = new Vector2(100f, 100f);

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
        coinText.text = CurrencyManager.Instance.GetCoinAmount().ToString();
    }
    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }
    public void ShowFailPanel()
    {
        failPanel.SetActive(true);

    }
    public void UpdateText(int amount)
    {
        int oldAmount = CurrencyManager.Instance.GetCoinAmount() - 50;
        DOTween.To(() => oldAmount, x =>
        {
            oldAmount = x;
            coinText.text = Mathf.FloorToInt(oldAmount).ToString(); 
        }, amount, 1f).SetEase(Ease.InOutQuad).SetDelay(1f);

    }
    public void StartCoinAnimation()
    {
        for (int i = 0; i < coinCount; i++)
        {

            GameObject coin = Instantiate(coinPrefab, startPoint.position, Quaternion.identity, coinParentTransform);
            RectTransform coinTransform = coin.GetComponent<RectTransform>();


            Vector2 randomOffset = new Vector2(
                Random.Range(-spreadRange.x, spreadRange.x),
                Random.Range(0f, spreadRange.y)
            );

            Sequence coinSequence = DOTween.Sequence();
            coinSequence.Append(coinTransform.DOAnchorPos(randomOffset, duration / 2).SetRelative(true).SetEase(Ease.OutQuad))  
                         .Append(coinTransform.DOMove(targetButton.position, duration / 2).SetEase(Ease.InQuad))  
                         .OnComplete(() => Destroy(coin));  
        }
    }
}
