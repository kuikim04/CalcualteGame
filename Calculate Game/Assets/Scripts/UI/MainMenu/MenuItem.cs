using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    [HideInInspector] public Image Img;
    [HideInInspector] public RectTransform rectTrans;

    ToggleMenu settingsMenu;

    Button button;

    int index;

    void Awake()
    {
        Img = GetComponent<Image>();
        rectTrans = GetComponent<RectTransform>();

        settingsMenu = rectTrans.parent.GetComponent<ToggleMenu>();
        index = rectTrans.GetSiblingIndex() - 1;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnItemClick);
    }

    void OnItemClick()
    {
        settingsMenu.OnItemClick(index);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(OnItemClick);
    }
}
