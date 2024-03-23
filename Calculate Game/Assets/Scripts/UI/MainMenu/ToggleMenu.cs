using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("Main button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    [Space]
    [Header("Page")]
    [SerializeField] GameObject settingPage;
    [SerializeField] OpenCloseAnimation settingAnim;
    [SerializeField] GameObject bestScorePage;
    [SerializeField] OpenCloseAnimation bestScoreAnim;
    [SerializeField] GameObject shopPage;
    [SerializeField] OpenCloseAnimation shopAnim;
    GameObject currentActivePage;


    Button mainButton;
    MenuItem[] menuItems;
    bool isExpanded = false;
    Vector2 mainButtonPosition;
    int itemsCount;

    void Start()
    {
        itemsCount = transform.childCount - 1;
        menuItems = new MenuItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i] = transform.GetChild(i + 1).GetComponent<MenuItem>();
        }

        mainButton = transform.GetChild(0).GetComponent<Button>();
        mainButton.onClick.AddListener(OpenMenu);
        mainButton.transform.SetAsLastSibling();

        mainButtonPosition = mainButton.GetComponent<RectTransform>().anchoredPosition;

        ResetPositions();
    }

    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.anchoredPosition = mainButtonPosition;
        }
    }

    void OpenMenu()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            SoundManager.Instance.OnClickSound();

            //menu opened
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                menuItems[i].Img.DOFade(1f, expandFadeDuration).From(0f);
            }
        }
        else
        {
            SoundManager.Instance.OnClickSound();

            //menu closed
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                menuItems[i].Img.DOFade(0f, collapseFadeDuration);
            }
        }

        mainButton.transform
              .DORotate(Vector3.forward * 180f, rotationDuration)
              .From(Vector3.zero)
              .SetEase(rotationEase);
    }
    public void OnItemClick(int index)
    {
        switch (index)
        {
            case 0:
                SoundManager.Instance.OnClickSound();
                settingAnim.OpenGameObject();
                currentActivePage = settingPage;
                break;
            case 1:
                SoundManager.Instance.OnClickSound();
                shopAnim.OpenGameObject();
                currentActivePage = shopPage;
                break;
            case 2:
                SoundManager.Instance.OnClickSound();
                bestScoreAnim.OpenGameObject();
                currentActivePage = bestScorePage;
                break;
        }
    }
    public void CloseCurrentPage()
    {
        if (currentActivePage != null && currentActivePage.activeSelf)
        {
            SoundManager.Instance.OnClickSound();

            if (currentActivePage == shopPage)
            {
                shopAnim.CloseGameObject();
            }
            else if (currentActivePage == settingPage)
            {

                settingAnim.CloseGameObject();
            }
            else 
            {
                bestScoreAnim.CloseGameObject();
            }

            currentActivePage = null;
        }
    }

    private void OnDestroy()
    {
        mainButton.onClick.RemoveListener(OpenMenu);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
