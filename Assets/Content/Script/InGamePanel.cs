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
    TextMeshProUGUI descText;
    [SerializeField]
    GameObject correctImage;
    [SerializeField]
    GameObject wrongImage;
    [SerializeField]
    CleanButton bookmarkBtnInDesc;

    //! 완료 패널
    [SerializeField]
    GameObject completePanel;
    [SerializeField]
    CleanButton completeBtn;
    [SerializeField]
    StarWidget[] stars;
    [SerializeField]
    TextMeshProUGUI scoreText;

    //! 공통
    public AudioClip successSound;
    public AudioClip failSound;
    public AudioClip quizShowSound;
    public AudioClip completeSound;
    int correctCount = 0;
    BookmarkComponent bookmarkComponent = new BookmarkComponent();

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
        bookmarkBtnInDesc.onClick.AddListener(OnBookmarkClicked);
        backBtn.onClick.AddListener(OnBackBtnClicked);
        completeBtn.onClick.AddListener(OnCompleteBtnClicked);
    }
    private void OnDestroy()
    {
        correctBtn.onClick.RemoveListener(OnCorrectClicked);
        wrongBtn.onClick.RemoveListener(OnWrongClicked);
        nextBtn.onClick.RemoveListener(OnNextClicked);
        bookmarkBtnInDesc.onClick.RemoveListener(OnBookmarkClicked);
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
        correctCount = 0;
        currentIndex = 0;
        QuizPanel.SetActive(true);
        descPanel.SetActive(false);
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
            correctCount += 1;
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

        float score = correctCount / (float)currentQuizList.Count * 100;
        scoreText.text = string.Format("{0:0.0}",score) + "점";

        if (score > 80)
        {
            StartCoroutine(PlayStars(3));
        }
        else if (score > 50)
        {
            StartCoroutine(PlayStars(2));
        }
        else
        {
            StartCoroutine(PlayStars(1));
        }
    }
    public void OnBookmarkClicked()
    {
        bookmarkComponent.AddBookmark(currentQuizList[currentIndex]);
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
        if (count >= 1)
        {
            stars[0].PlayAnimation();
        }
        else
        {
            stars[0].PlayDisableAnimation();
        }
        yield return new WaitForSeconds(1);

        if (count >= 2)
        {
            stars[1].PlayAnimation();
        }
        else
        {
            stars[1].PlayDisableAnimation();
        }
        yield return new WaitForSeconds(1);

        if (count >= 3)
        {
            stars[2].PlayAnimation();
        }
        else
        {
            stars[2].PlayDisableAnimation();
        }
        yield return new WaitForSeconds(3);
        completeBtn.enabled = true;
    }
}
