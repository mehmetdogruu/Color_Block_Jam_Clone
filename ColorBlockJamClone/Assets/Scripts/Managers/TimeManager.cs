using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float timerDuration = 30f;  
    private float currentTime;
    public bool timerActive = false;

    [SerializeField] private TMP_Text timerText;
    public static TimeManager Instance { get; private set; }

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
        StartTimer();  
    }

    private void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;

            UpdateTimerDisplay(currentTime);

            if (currentTime <= 0f)
            {
                timerActive = false;
                TimerEnded();
            }
        }
    }

    public void StartTimer()
    {
        currentTime = timerDuration;
        timerActive = true;
        UpdateTimerDisplay(currentTime); 
    }

    private void TimerEnded()
    {
        timerText.text = "Failed";  
        UIManager.Instance.ShowFailPanel();
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  
    }
}
