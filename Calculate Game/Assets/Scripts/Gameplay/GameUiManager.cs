using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{

    [SerializeField] private OpenCloseAnimation settingAnim;
    [SerializeField] private Button openSettingButton;
    [SerializeField] private Button closeSettingButton;

    [Header("ITEM")]
    [SerializeField] private TextMeshProUGUI[] textItems;
    [SerializeField] private Button[] itemButtons;

    private void Start()
    {
        UpdateItemUI();
        InitializeItemButtons();
    }

    private void OnDestroy()
    {
        RemoveItemButtonListeners();
    }

    private void Update()
    {
        UpdateItemUI();
    }

    private void OnEnable()
    {
        openSettingButton.onClick.AddListener(OpenSetting);
        closeSettingButton.onClick.AddListener(CloseSetting);
    }

    private void OnDisable()
    {
        openSettingButton.onClick.RemoveListener(OpenSetting);
        closeSettingButton.onClick.RemoveListener(CloseSetting);
    }

    private void UpdateItemUI()
    {
        for (int i = 0; i < textItems.Length; i++)
        {
            textItems[i].text = DataCenter.Instance.GetItemData(i + 1).ToString();
            itemButtons[i].interactable = DataCenter.Instance.GetItemData(i + 1) > 0;
        }
    }

    private void InitializeItemButtons()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int itemId = i + 1;
            itemButtons[i].onClick.AddListener(() => GameManager.Instance.UseItem(itemId));
        }
    }

    private void RemoveItemButtonListeners()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].onClick.RemoveAllListeners();
        }
    }

    public void OpenSetting()
    {
        settingAnim.OpenGameObject();
        StartCoroutine(StopTime());
    }

    public void CloseSetting()
    {
        Time.timeScale = 1f;
        settingAnim.CloseGameObject();
    }

    IEnumerator StopTime()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }
}
