using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private int choiceIndex;
    private GameManager gameManager;
    private Button button;

    private void Start()
    {
        gameManager = GameManager.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (gameManager == null)
        {
            return;
        }

        gameManager.OnChoiceSelected(choiceIndex);
    }
}
