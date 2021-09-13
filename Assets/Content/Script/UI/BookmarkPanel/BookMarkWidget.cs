using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class BookMarkWidget : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image outline;
    [SerializeField]
    Image answerBtnImage;
    [SerializeField]
    Image removeBookmarkBtnImage;
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
    UltimateClean.CleanButton removeBookmarkButton;
    [SerializeField]
    TextMeshProUGUI buttonText;

    DataManager.QuizData currentQuizData = new DataManager.QuizData();
    public UnityEvent OnNeedRefreshPanel = new UnityEvent(); 
    private enum State
    {
        quiz,
        desc
    }
    State state = State.quiz;

    string[] colorList = { "#042159", "#F28A2E", "#F23827", "#BF548F", "#3449BF" };
    void Start()
    {
        Color color = Color.gray;
        ColorUtility.TryParseHtmlString(colorList[Random.Range(0, colorList.Length)], out color);
        outline.color = color;
        answerBtnImage.color = color;
        removeBookmarkBtnImage.color = color;

        changeStateButton.onClick.AddListener(OnChangeStateBtnClicked);
        removeBookmarkButton.onClick.AddListener(OnRemoveBookmarkBtnClicked);
        UpdateUI();
    }
    private void OnDestroy()
    {
        changeStateButton.onClick.RemoveListener(OnChangeStateBtnClicked);
        removeBookmarkButton.onClick.RemoveListener(OnRemoveBookmarkBtnClicked);
    }

    public void Initialized(DataManager.QuizData quizData)
    {
        currentQuizData = quizData;

        state = State.quiz;
        chapterUI.text = quizData.category;
        quizUI.text = quizData.quiz;
        descUI.text = quizData .isAnswer? "O" : "X";
        if(false == string.IsNullOrEmpty(quizData.description))
        {
            descUI.text += " , " + quizData.description;
        }
        sourceUI.text = "";

        UpdateUI();
    }

    void OnRemoveBookmarkBtnClicked()
    {
        if(BookmarkComponent.RemoveBookmark(currentQuizData))
        {
            OnNeedRefreshPanel.Invoke();
        }
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
