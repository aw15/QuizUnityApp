using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : IPanel
{
    [SerializeField]
    CleanButton backBtn;
    [SerializeField]
    Image background;
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
    GameObject descPanel;
    [SerializeField]
    CleanButton nextBtn;
    [SerializeField]
    CleanButton bookmarkBtn;
    [SerializeField]
    TextMeshProUGUI descText;
    [SerializeField]
    GameObject correctImage;
    [SerializeField]
    GameObject wrongImage;

    //! 완료 패널
    [SerializeField]
    GameObject completePanel;
    [SerializeField]
    CleanButton completeBtn;
    [SerializeField]
    StarWidget[] stars;

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
        descPanel.SetActive(false);
        completePanel.SetActive(false);
        background.color = Color.white;
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
    void ChangeToDescPanel(bool isAnswer)
    {
        if (isAnswer)
        {
            audioSource.clip = successSound;
            correctImage.SetActive(true);
            wrongImage.SetActive(false);
        }
        else
        {
            audioSource.clip = failSound;
            correctImage.SetActive(false);
            wrongImage.SetActive(true);
        }

        audioSource.Play();

        descText.text = currentQuizList[currentIndex].description;

        QuizPanel.SetActive(false);
        descPanel.SetActive(true);
    }

    void ChangeToQuizPanel()
    {
        currentIndex += 1;
        if (currentIndex < currentQuizList.Count)
        {
            SelectQuiz(currentIndex);
            QuizPanel.SetActive(true);
            descPanel.SetActive(false);
        }
        else//! 문제를 모두 푼 경우
        {
            ChangeToCompletePanel();
        }
    }
    void ChangeToCompletePanel()
    {
        descPanel.SetActive(false);
        completePanel.SetActive(true);
        audioSource.clip = completeSound;
        audioSource.Play();
        backBtn.enabled = false;
        foreach (var star in stars)
            star.visible = false;
        StartCoroutine(PlayStars(3));
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
        UIManager.Ins.PopPanel();
    }
    public void OnCompleteBtnClicked()
    {
        UIManager.Ins.PopPanel();
    }

    IEnumerator PlayStars(int count)
    {
        completeBtn.enabled = false;
        //Color color = Color.white;
        //ColorUtility.TryParseHtmlString("#262626", out color);
        //background.color = color;
        stars[0].PlayAnimation();
        yield return new WaitForSeconds(1);
        stars[1].PlayAnimation();
        yield return new WaitForSeconds(1);
        stars[2].PlayAnimation();
        yield return new WaitForSeconds(3);
        completeBtn.enabled = true;
    }
}
