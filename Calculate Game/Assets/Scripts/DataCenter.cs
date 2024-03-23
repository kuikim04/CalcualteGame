using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenter : MonoBehaviour
{
    public static DataCenter Instance;

    [SerializeField] private PlayerData data;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public PlayerData GetPlayerData()
    {
        return data;
    }

    public void LoadAllData()
    {
        LoadCurrency();
        LoadItemData();
        LoadBestScore();
    }

    #region Currency

    public int GetPlayerCoinData()
    {
        return data.Coin;
    }

    public void IncreaseCoin(int amount)
    {
        data.Coin += amount;
        SaveCurrency();
    }

    public void DecreaseCoin(int amount)
    {
        data.Coin -= amount;
        SaveCurrency();
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(Key.KEY_COIN, data.Coin);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        data.Coin = PlayerPrefs.GetInt(Key.KEY_COIN);
    }

    #endregion

    #region Item

    public int GetItemData(int itemId)
    {
        switch (itemId)
        {
            case 1: return data.Item1;
            case 2: return data.Item2;
            case 3: return data.Item3;
            case 4: return data.Item4;
            default: return 0;
        }
    }

    public void IncreaseItem(int itemId, int amount)
    {
        switch (itemId)
        {
            case 1: data.Item1 += amount; break;
            case 2: data.Item2 += amount; break;
            case 3: data.Item3 += amount; break;
            case 4: data.Item4 += amount; break;
        }
        SaveItemData();
    }

    public void DecreaseItem(int itemId, int amount)
    {
        switch (itemId)
        {
            case 1: data.Item1 -= amount; break;
            case 2: data.Item2 -= amount; break;
            case 3: data.Item3 -= amount; break;
            case 4: data.Item4 -= amount; break;
        }
        SaveItemData();
    }

    public void SaveItemData()
    {
        PlayerPrefs.SetInt(Key.KEY_ITEM_PREFIX + "1", data.Item1);
        PlayerPrefs.SetInt(Key.KEY_ITEM_PREFIX + "2", data.Item2);
        PlayerPrefs.SetInt(Key.KEY_ITEM_PREFIX + "3", data.Item3);
        PlayerPrefs.SetInt(Key.KEY_ITEM_PREFIX + "4", data.Item4);

        PlayerPrefs.Save();
    }

    private void LoadItemData()
    {
        data.Item1 = PlayerPrefs.GetInt(Key.KEY_ITEM_PREFIX + "1");
        data.Item2 = PlayerPrefs.GetInt(Key.KEY_ITEM_PREFIX + "2");
        data.Item3 = PlayerPrefs.GetInt(Key.KEY_ITEM_PREFIX + "3");
        data.Item4 = PlayerPrefs.GetInt(Key.KEY_ITEM_PREFIX + "4");
    }

    #endregion

    #region Best Score

    public int GetPlayerBestScoreData()
    {
        return LoadBestScore();
    }

    public void SaveBestScore(int bestScore)
    {
        PlayerPrefs.SetInt(Key.KEY_BESTSCORE, bestScore);
        PlayerPrefs.Save();
    }

    private int LoadBestScore()
    {
        return PlayerPrefs.GetInt(Key.KEY_BESTSCORE);
    }

    #endregion


}
