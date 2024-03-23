using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;

    [SerializeField] private Button replayBtn;
    [SerializeField] private Button mainmenuBtn;

    [SerializeField] private GameObject bestScoreObj;
    private void OnEnable()
    {
        replayBtn.onClick.AddListener(Replay);
        mainmenuBtn.onClick.AddListener(BackMainMenu);
    }
    private void OnDisable()
    {

        replayBtn.onClick.RemoveListener(Replay);
        mainmenuBtn.onClick.RemoveListener(BackMainMenu);
    }

    public void Init(int score)
    {
        if (score > DataCenter.Instance.GetPlayerBestScoreData())
        {
            bestScoreObj.SetActive(true);
            textScore.text = score.ToString();
            DataCenter.Instance.SaveBestScore(score);
            DataCenter.Instance.IncreaseCoin(Random.Range(score, score*2));
        }
        else
        {
            bestScoreObj.SetActive(false);
            textScore.text = score.ToString();
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(Key.KEY_GAMESCENE);
    }
    public void BackMainMenu()
    {
        SoundManager.Instance.OnMainSound();
        SceneManager.LoadScene(Key.KEY_MAINSCENE);
    }
}
