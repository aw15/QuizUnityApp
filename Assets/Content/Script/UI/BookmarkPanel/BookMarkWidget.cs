using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BookMarkWidget : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image outline;
    [SerializeField]
    Image answerBtnImage;
    [SerializeField]
    TextMeshProUGUI chapterUI;
    [SerializeField]
    TextMeshProUGUI quizUI;
    [SerializeField]
    TextMeshProUGUI descUI;
    [SerializeField]
    TextMeshProUGUI sourceUI;
    [SerializeField]
    UltimateClean.CleanButton changeStateButton;
    [SerializeField]
    TextMeshProUGUI buttonText;

    private enum State
    {
        quiz,
        desc
    }
    State state;

    string[] colorList = { "#042159", "#F28A2E", "#F23827", "#BF548F", "#3449BF" };
    void Start()
    {
        Color color = Color.gray;
        Debug.Log(Random.Range(0, colorList.Length));
        ColorUtility.TryParseHtmlString(colorList[Random.Range(0, colorList.Length)], out color);
        outline.color = color;
        answerBtnImage.color = color;

        changeStateButton.onClick.AddListener(OnChangeStateBtnClicked);
    }
    private void OnDestroy()
    {
        changeStateButton.onClick.RemoveListener(OnChangeStateBtnClicked);
    }

    public void Initialized(DataManager.QuizData quizData)
    {
        state = State.quiz;
        chapterUI.text = quizData.category;
        quizUI.text = quizData.quiz;
        descUI.text = quizData .isAnswer? "O , " : "X , " + quizData.description;
        sourceUI.text = quizData.source;

        UpdateUI();
    }
    public void OnChangeStateBtnClicked()
    {
        state = state == State.quiz ? State.desc : State.quiz;
        UpdateUI();
    }
    void UpdateUI()
    {
        if(state == State.quiz)
        {
            quizUI.enabled = true;
            chapterUI.enabled = true;
            sourceUI.enabled = false;
            descUI.enabled = false;
        }
        else
        {
            quizUI.enabled = false;
            chapterUI.enabled = false;
            sourceUI.enabled = true;
            descUI.enabled = true;
        }
    }
}
