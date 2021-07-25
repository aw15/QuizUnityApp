using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;

public class InGamePanel : IPanel
{
    [SerializeField]
    CleanButton backBtn;
    //! 퀴즈 패널
    [SerializeField]
    GameObject QuizPanel;
    [SerializeField]
    TextMeshProUGUI countUIOnQuiz;
    [SerializeField]
    CleanButton correctBtn;
    [SerializeField]
    CleanButton wrongBtn;
    [SerializeField]
    TextMeshProUGUI sourceText;
    [SerializeField]
    TextMeshProUGUI quizText;
    List<DataManager.QuizData> currentQuizList = new List<DataManager.QuizData>();
    int currentIndex = 0;

    //! 해설 패널
    [SerializeField]
    GameObject DescPanel;
    [SerializeField]
    CleanButton nextBtn;
    [SerializeField]
    CleanButton bookmarkBtn;
    [SerializeField]
    TextMeshProUGUI descText;
    [SerializeField]
    TextMeshProUGUI wrongHighlight;
    [SerializeField]
    TextMeshProUGUI countUIOnDesc;

    //! 완료 패널
    [SerializeField]
    GameObject completePanel;
    [SerializeField]
    CleanButton completeBtn;
    [SerializeField]
    GameObject dinoSprite;

    //! 공통
    public AudioClip successSound;
    public AudioClip failSound;
    public AudioClip quizShowSound;
    public AudioClip completeSound;

    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        UIManager.Ins.AddPanel("InGamePanel", this);
        Hide();
        correctBtn.onClick.AddListener(OnCorrectClicked);
        wrongBtn.onClick.AddListener(OnWrongClicked);
        nextBtn.onClick.AddListener(OnNextClicked);
        bookmarkBtn.onClick.AddListener(OnBookmarkClicked);
        backBtn.onClick.AddListener(OnBackBtnClicked);
        completeBtn.onClick.AddListener(OnCompleteBtnClicked);
    }
    private void OnDestroy()
    {
        correctBtn.onClick.RemoveListener(OnCorrectClicked);
        wrongBtn.onClick.RemoveListener(OnWrongClicked);
        nextBtn.onClick.RemoveListener(OnNextClicked);
        bookmarkBtn.onClick.RemoveListener(OnBookmarkClicked);
        backBtn.onClick.RemoveListener(OnBackBtnClicked);
        completeBtn.onClick.RemoveListener(OnCompleteBtnClicked);
    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        UIManager.Ins.AddPanel("InGamePanel", this);
    }
# endif
    void Clear()
    {
        currentIndex = 0;
        QuizPanel.SetActive(true);
        DescPanel.SetActive(false);
        completePanel.SetActive(false);
    }

    public void OnGameStart(string categoryName) //진입점.
    {
        Clear();
        currentQuizList = DataManager.Ins.quizDatabase.data[categoryName];
        Utils.Shuffle<DataManager.QuizData>(currentQuizList);
        SelectQuiz(currentIndex);
        correctBtn.enabled = currentQuizList.Count > 0;
        wrongBtn.enabled = currentQuizList.Count > 0;
        backBtn.enabled = true;
    }
    private void SelectQuiz(int index)
    {
        if (index < currentQuizList.Count)
        {
            quizText.text = currentQuizList[index].quiz;
            sourceText.text = currentQuizList[index].source;
            audioSource.clip = quizShowSound;
            audioSource.Play();
        }
        countUIOnQuiz.text = $"문제 {index + 1}/{currentQuizList.Count}";
        countUIOnDesc.text = $"문제 {index + 1}/{currentQuizList.Count}";
    }
    public void OnCorrectClicked()
    {
        ChangeToDescPanel(currentQuizList[currentIndex].isAnswer == true);
    }
    public void OnWrongClicked()
    {
        ChangeToDescPanel(currentQuizList[currentIndex].isAnswer == false);
    }
    public void OnNextClicked()
    {
        ChangeToQuizPanel();
    }
    public void ChangeToDescPanel(bool isAnswer)
    {
        if (isAnswer)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#04CFBC", out color);
            wrongHighlight.color = color;
            wrongHighlight.text = "성공!";
            audioSource.clip = successSound;
        }
        else
        {
            wrongHighlight.color = Color.red;
            wrongHighlight.text = "실패!";
            audioSource.clip = failSound;
        }

        audioSource.Play();

        descText.text = currentQuizList[currentIndex].description;

        QuizPanel.SetActive(false);
        DescPanel.SetActive(true);
    }

    public void ChangeToQuizPanel()
    {
        currentIndex += 1;
        if (currentIndex < currentQuizList.Count)
        {
            SelectQuiz(currentIndex);
            QuizPanel.SetActive(true);
            DescPanel.SetActive(false);
        }
        else
        {
            DescPanel.SetActive(false);
            completePanel.SetActive(true);
            dinoSprite.SetActive(true);
            audioSource.clip = completeSound;
            audioSource.Play();
            backBtn.enabled = false;
        }
    }
    public void OnBookmarkClicked()
    {

    }
    public override void Show()
    {
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
    }
    public void OnBackBtnClicked()
    {
        dinoSprite.SetActive(false);
        UIManager.Ins.PopPanel();
    }
    public void OnCompleteBtnClicked()
    {
        dinoSprite.SetActive(false);
        UIManager.Ins.PopPanel();
    }
}
