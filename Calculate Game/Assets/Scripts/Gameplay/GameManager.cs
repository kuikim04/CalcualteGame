using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Singleton Instance")]
    public static GameManager Instance;

    [Header("Game Time")]
    [SerializeField] private CountDownTime gameTime;

    [Header("Score")]
    public int Score;
    public TextMeshProUGUI scoreText;

    [Header("Question UI")]
    public TextMeshProUGUI QuestionText;

    [Header("Choice UI")]
    public TextMeshProUGUI[] ChoiceTexts;
    public Button[] ChoiceButton;

    private List<Question> questions;
    private int currentQuestionIndex = 0;

    [Header("UI Objects")]
    [SerializeField] private GameObject showAnswerObj;
    [SerializeField] private TextMeshProUGUI showAnswerText;

    [Header("Time Item")]
    [SerializeField] private GameObject timeItemObj;
    [SerializeField] private Transform timeItemConten;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        GenerateQuestions();
        DisplayQuestion();
        scoreText.text = Score.ToString();
    }

    private void GenerateQuestions()
    {
        questions = new List<Question>();
        HashSet<string> generatedQuestions = new HashSet<string>(); 
        float timeRemaining = gameTime.timeGame;

        while (questions.Count < currentQuestionIndex + 1)
        {
            float num1, num2;
            float answer;
            string question;

            int operation = Random.Range(0, 4);

            int minNumber = 1;
            int maxNumber = 10;

            if (timeRemaining < 60)
            {
                minNumber = 1;
                maxNumber = 1000;
            }

            switch (operation)
            {
                case 0: // บวก
                    num1 = Random.Range(minNumber, maxNumber);
                    num2 = Random.Range(minNumber, maxNumber);
                    answer = num1 + num2;
                    question = num1 + " + " + num2 + " = ?";
                    break;
                case 1: // ลบ
                    num1 = Random.Range(minNumber, maxNumber);
                    num2 = Random.Range(minNumber, maxNumber);
                    answer = num1 - num2;
                    question = num1 + " - " + num2 + " = ?";
                    break;
                case 2: // คูณ
                    num1 = Random.Range(minNumber, maxNumber);
                    num2 = Random.Range(minNumber, maxNumber);
                    answer = num1 * num2;
                    question = num1 + " × " + num2 + " = ?";
                    break;
                case 3: // หาร
                    num1 = Random.Range(minNumber, maxNumber);
                    num2 = Random.Range(minNumber, maxNumber); 
                    float floatResult = (float)num1 / (float)num2;
                    floatResult = Mathf.Round(floatResult * 100f) / 100f;
                    answer = floatResult; 
                    question = num1 + " ÷ " + num2 + " = ?";
                    break;
                default:
                    answer = 0;
                    question = "";
                    break;
            }

            if (!generatedQuestions.Contains(question))
            {
                generatedQuestions.Add(question);

                Question newQuestion = new Question();
                newQuestion.question = question;
                newQuestion.answer = answer;
                newQuestion.choice = Random.Range(0, 4);

                questions.Add(newQuestion);
            }
        }
    }


    private void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            QuestionText.text = questions[currentQuestionIndex].question;

            List<int> choiceIndices = new List<int> { 0, 1, 2, 3 };
            choiceIndices.Remove(questions[currentQuestionIndex].choice);
            Shuffle(choiceIndices);

            for (int i = 0; i < 4; i++)
            {
                ChoiceTexts[i].text = (i == questions[currentQuestionIndex].choice) ?
                    questions[currentQuestionIndex].answer.ToString() :
                    (questions[currentQuestionIndex].answer % 1 == 0) ?
                    GenerateWrongAnswer().ToString() :
                    GenerateWrongAnswerFloat().ToString();
            }
        }
    }


    private int GenerateWrongAnswer()
    {
        int correctAnswer = (int)questions[currentQuestionIndex].answer;
        int wrongAnswer = Random.Range(correctAnswer - 10, correctAnswer + 10);

        while (wrongAnswer == correctAnswer)
        {
            wrongAnswer = Random.Range(correctAnswer - 10, correctAnswer + 10);
        }

        return wrongAnswer;
    }

    private float GenerateWrongAnswerFloat()
    {
        float correctAnswer = questions[currentQuestionIndex].answer;
        float wrongAnswer = Random.Range(correctAnswer - 10f, correctAnswer + 10f);

        while (Mathf.Approximately(wrongAnswer, correctAnswer))
        {
            wrongAnswer = Random.Range(correctAnswer - 10f, correctAnswer + 10f);
        }

        return Mathf.Round(wrongAnswer * 100f) / 100f;
    }


    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void IncreaseScore()
    {
        float timeRemaining = gameTime.timeGame;
        SoundManager.Instance.OnTrueChoiceSound();

        if (timeRemaining <= 60)
        {
            Score += 100;
        }
        else
        {
            Score ++;
        }

        scoreText.text = Score.ToString();
    }

    public void DecreaseScore()
    {
        float timeRemaining = gameTime.timeGame;
        SoundManager.Instance.OnWrongChoiceSound();

        if (Score <= 0)
            return;

        if (timeRemaining <= 60)
        {
            Score -= 100;
        }
        else
        {
            Score --;
        }

        scoreText.text = Score.ToString();
    }

    public bool CheckAnswer(int choiceIndex)
    {
        if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Count)
        {
            return choiceIndex == questions[currentQuestionIndex].choice;
        }
        else
        {
            Debug.LogError("Index out of range.");
            return false;
        }
    }

    public void OnChoiceSelected(int choiceIndex)
    {
        if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Count)
        {
            bool isCorrect = CheckAnswer(choiceIndex);
            if (isCorrect)
            {
                IncreaseScore();
            }
            else
            {
                DecreaseScore();
            }
            currentQuestionIndex++;
            GenerateQuestions();
            DisplayQuestion();
        }
        else
        {
            Debug.LogError("Index out of range.");
        }
    }

    #region USE ITEM

    public void UseItem(int itemID)
    {
        DataCenter.Instance.DecreaseItem(itemID, 1);

        switch (itemID)
        {
            case 1:
                SoundManager.Instance.OnClickSound();
                StartCoroutine(TimeItem());
                break;
            case 2:
                SoundManager.Instance.OnClickSound();
                GenerateQuestions();
                DisplayQuestion();
                break;
            case 3:
                SoundManager.Instance.OnClickSound();
                StartCoroutine(ShowAnswerItem());
                break;
            case 4:
                SoundManager.Instance.OnClickSound();
                GenerateQuestions();
                DisplayQuestion();
                IncreaseScore();
                break;
            default:
                break;
        }
    }

    IEnumerator ShowAnswerItem()
    {
        showAnswerObj.SetActive(true);
        showAnswerText.text = questions[currentQuestionIndex].question + "\n " + questions[currentQuestionIndex].answer;
        yield return new WaitForSeconds(2);
        showAnswerObj.SetActive(false);
        showAnswerText.text = "";

    }
    IEnumerator TimeItem()
    {
        var obj = Instantiate(timeItemObj);
        obj.transform.SetParent(timeItemConten);
        obj.transform.localPosition = Vector3.zero;
        gameTime.timeGame += 10;
        SoundManager.Instance.StopSoundEffect();

        yield return new WaitForSeconds(0.5f);
        Destroy(obj);

    }

    #endregion

}
