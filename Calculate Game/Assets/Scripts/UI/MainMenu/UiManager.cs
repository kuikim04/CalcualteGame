using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Space]
    [Header("BUTTON")]
    [Header("BUTTON MAIN")]
    [SerializeField] private Button playGameButton;

    [Header("BUTTON SHOP")]
    [SerializeField] private Button buyItem1Button;
    [SerializeField] private Button buyItem2Button;
    [SerializeField] private Button buyItem3Button;
    [SerializeField] private Button buyItem4Button;

    
    [Space]
    [Header("TEXT")]
    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI textCoin;

    [Header("ItemAmount")]
    [SerializeField] private TextMeshProUGUI textItem1;
    [SerializeField] private TextMeshProUGUI textItem2;
    [SerializeField] private TextMeshProUGUI textItem3;
    [SerializeField] private TextMeshProUGUI textItem4;

    [Header("ItemPrice")]
    [SerializeField] private TextMeshProUGUI textPriceItem1;
    [SerializeField] private TextMeshProUGUI textPriceItem2;
    [SerializeField] private TextMeshProUGUI textPriceItem3;
    [SerializeField] private TextMeshProUGUI textPriceItem4;

    [Header("ItemPrice")]
    [SerializeField] private TextMeshProUGUI textBestScore;

    [Space]
    [Header("PAGE")]
    [SerializeField] private GameObject shopPage;


    [Space]
    [Header("INT")]
    [SerializeField] private int priceItem1;
    [SerializeField] private int priceItem2;
    [SerializeField] private int priceItem3;
    [SerializeField] private int priceItem4;

    private void Start()
    {
        DataCenter.Instance.LoadAllData();

        UpdateCoinText();
        UpdateItemTexts();

        InitialButtonShop();
        InitialButtonBestScore();

    }


    private void OnEnable()
    {
        playGameButton.onClick.AddListener(PlayGame);
    }
    private void OnDisable()
    {
        playGameButton.onClick.RemoveListener(PlayGame);
    }

    #region PLAYGAME

    private void PlayGame()
    {
        StartCoroutine(LoadGameSceneWithDelay());
    }

    private IEnumerator LoadGameSceneWithDelay()
    {
        SoundManager.Instance.OnClickSound();
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.StopBackgroundMusic();
        SceneManager.LoadScene(Key.KEY_GAMESCENE);
    }

    #endregion

    #region SHOP
    private void InitialButtonShop()
    {
        buyItem1Button.onClick.AddListener(() => BuyItem(1));
        buyItem2Button.onClick.AddListener(() => BuyItem(2));
        buyItem3Button.onClick.AddListener(() => BuyItem(3));
        buyItem4Button.onClick.AddListener(() => BuyItem(4));
    }
    public void BuyItem(int itemID)
    {
        switch (itemID)
        {
            case 1:
                CheckBuyItem(1, priceItem1); 
                UpdateCoinText();
                UpdateItemTexts();
                break;
            case 2:
                CheckBuyItem(2, priceItem2);
                UpdateCoinText();
                UpdateItemTexts();
                break;
            case 3:
                CheckBuyItem(3, priceItem3);
                UpdateCoinText();
                UpdateItemTexts();
                break;
            case 4:
                CheckBuyItem(4, priceItem4);
                UpdateCoinText();
                UpdateItemTexts();
                break;
            default:
                break;
        }
    }
    private void CheckBuyItem(int itemID, int itemPrice)
    {
        if(DataCenter.Instance.GetPlayerCoinData() < itemPrice)
        {
            SoundManager.Instance.OnNoMoneySound();
            return;
        }

        SoundManager.Instance.OnBuySound();
        DataCenter.Instance.DecreaseCoin(itemPrice);
        DataCenter.Instance.IncreaseItem(itemID, 1);
    }
    #endregion

    #region BEST SCORE

    private void InitialButtonBestScore()
    {
        textBestScore.text = DataCenter.Instance.GetPlayerBestScoreData().ToString();   
    }

    #endregion

    private void UpdateCoinText()
    {
        int coin = DataCenter.Instance.GetPlayerCoinData();
        string formattedCoin = FormatNumber(coin);
        textCoin.text = formattedCoin;
    }
    private void UpdateItemTexts()
    {
        textPriceItem1.text = priceItem1.ToString();
        textPriceItem2.text = priceItem2.ToString();
        textPriceItem3.text = priceItem3.ToString();
        textPriceItem4.text = priceItem4.ToString();

        textItem1.text = FormatNumber(DataCenter.Instance.GetItemData(1));
        textItem2.text = FormatNumber(DataCenter.Instance.GetItemData(2));
        textItem3.text = FormatNumber(DataCenter.Instance.GetItemData(3));
        textItem4.text = FormatNumber(DataCenter.Instance.GetItemData(4));
    }
    private string FormatNumber(int num)
    {
        if (num >= 1000)
        {
            float k = num / 1000f;
            return k.ToString("0.0") + "k";
        }
        else
        {
            return num.ToString();
        }
    }
}
