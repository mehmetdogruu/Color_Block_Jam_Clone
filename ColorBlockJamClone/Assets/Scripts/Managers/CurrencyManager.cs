using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

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

    public void UpdateCoin(int amount)
    {
        PlayerPrefs.SetInt("Coin", amount);
        PlayerPrefs.Save();
    }
    public int GetCoinAmount()
    {
        return PlayerPrefs.GetInt("Coin", 0);
    }
    public void AddCoin(int amount)
    {
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + amount);
        PlayerPrefs.Save();

    }

}
