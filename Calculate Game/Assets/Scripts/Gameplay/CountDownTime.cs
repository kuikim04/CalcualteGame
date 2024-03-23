using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownTime : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI timeCountDownStartText;
    [SerializeField] private GameObject timeCountDownStartObj;
    [SerializeField] private GameObject timeBonusObj;

    [Header("Time Settings")]
    public float countDownStart = 3f;
    public float timeGame;
    public float warningTime = 10f;

    private bool timeUp = false;
    private bool timeupSoundPlayed = false;
    private bool warningSoundPlayed = false;

    [Header("Result Animation")]
    public OpenCloseAnimation resultAnim;
    public ResultUI resultUI;

    private void Start()
    {
        timeCountDownStartObj.SetActive(true);
        StartCoroutine(StartCountDown());
    }

    private void Update()
    {
        UpdateTimeBonus();
    }

    private void UpdateTimeBonus()
    {
        if (timeGame <= 60f && !warningSoundPlayed)
        {
            timeBonusObj.SetActive(true);
            warningSoundPlayed = true;
            SoundManager.Instance.OnWarningSound();
        }
        else if (timeGame > 60f)
        {
            warningSoundPlayed = false;
            timeBonusObj.SetActive(false);
        }
    }

    private IEnumerator StartCountDown()
    {
        SoundManager.Instance.OnPlayCountdownSound();
        float count = countDownStart;
        while (count > 0)
        {
            timeCountDownStartText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        timeCountDownStartText.text = "Start";
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.OnGamePlaySound();
        timeCountDownStartObj.SetActive(false);
        StartCoroutine(StartGameTimer());
    }

    private IEnumerator StartGameTimer()
    {
        while (!timeUp)
        {
            UpdateTimeGame();
            yield return null;
        }
    }

    private void UpdateTimeGame()
    {
        timeGame -= Time.deltaTime;

        if (timeGame <= 0f)
        {
            timeGame = 0f;
            timeUp = true;
            HandleTimeUp();
        }
        else if (timeGame <= warningTime && !timeupSoundPlayed)
        {
            HandleWarningTime();
        }
        else if (timeGame > warningTime)
        {
            HandleNormalTime();
        }

        UpdateTimeDisplay();
    }

    private void HandleTimeUp()
    {
        SoundManager.Instance.StopSoundEffect();
        StartCoroutine(ShowResultScore());
    }

    private void HandleWarningTime()
    {
        SoundManager.Instance.OnPlayTimeupCountdownSound();
        timeText.color = Color.red;
        timeupSoundPlayed = true;
    }

    private void HandleNormalTime()
    {
        timeText.color = Color.black;
        timeupSoundPlayed = false;
    }

    private void UpdateTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(timeGame / 60);
        int seconds = Mathf.FloorToInt(timeGame % 60);

        minutes = Mathf.Max(minutes, 0);
        seconds = Mathf.Max(seconds, 0);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator ShowResultScore()
    {
        SoundManager.Instance.StopBackgroundMusic();
        yield return new WaitForSeconds(0.5f);

        resultAnim.OpenGameObject();
        resultUI.Init(GameManager.Instance.Score);

        if (GameManager.Instance.Score > DataCenter.Instance.GetPlayerBestScoreData())
            SoundManager.Instance.OnNewBestScoreSound();
        else
            SoundManager.Instance.OnShowResultSound();
    }
}
