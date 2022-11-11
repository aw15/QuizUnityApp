using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.Events;
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
    CleanButton bookmarkBtn;

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
    public AudioClip completeFailSound;
    int correctCount = 0;
    bool backBlock = false;
    [SerializeField]
    AudioSource audioSource;
    int stage = -1;
    int score = 0;
    public UnityEvent<int, int, bool> onTestCompleted = new UnityEvent<int, int, bool>();
    //타이머
    float playTime = 0f;
    float completeTime = 0f;
    void Start()
    {
        Hide();

        correctBtn.onClick.AddListener(OnCorrectClicked);
        wrongBtn.onClick.AddListener(OnWrongClicked);
        nextBtn.onClick.AddListener(OnNextClicked);
        bookmarkBtn.onClick.AddListener(OnBookmarkClicked);
        backBtn.onClick.AddListener(OnBackEvent);
        completeBtn.onClick.AddListener(OnCompleteBtnClicked);
    }
    private void Update()
    {
        playTime += Time.deltaTime;
    }
    private void OnDestroy()
    {
        correctBtn.onClick.RemoveListener(OnCorrectClicked);
        wrongBtn.onClick.RemoveListener(OnWrongClicked);
        nextBtn.onClick.RemoveListener(OnNextClicked);
        bookmarkBtn.onClick.RemoveListener(OnBookmarkClicked);
        backBtn.onClick.RemoveListener(OnBackEvent);
        completeBtn.onClick.RemoveListener(OnCompleteBtnClicked);
    }
    void Clear()
    {
        correctCount = 0;
        currentIndex = 0;
        QuizPanel.SetActive(true);
        descPanel.SetActive(false);
        completePanel.SetActive(false);
        backBlock = false;
        backBtn.enabled = true;
        playTime = 0f;
        completeTime = 0f;
    }

    public void OnGameStart(int stage, int startIndex, int lastIndex) //진입점.
    {
        Clear();
        this.stage = stage;
        currentQuizList = DataManager.Ins.quizDatabase.Data.GetRange(startIndex, lastIndex - startIndex + 1);//마지막 미포함
        SelectQuiz(currentIndex);
        correctBtn.enabled = currentQuizList.Count > 0;
        wrongBtn.enabled = currentQuizList.Count > 0;      
    }
    private void SelectQuiz(int index)
    {
        if (index < currentQuizList.Count)
        {
            quizText.text = currentQuizList[index].quiz;
            sourceText.text = "";
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
    void UpdateBookmarkBtnVisibleState()
    {
        bookmarkBtn.gameObject.SetActive(!BookmarkComponent.IsContainBookmark(currentQuizList[currentIndex]));
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

        UpdateBookmarkBtnVisibleState();

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
        //플레이 타임 기록
        completeTime = playTime;

        descPanel.SetActive(false);
        completePanel.SetActive(true);

        foreach (var star in stars)
            star.visible = false;

        float score = correctCount / (float)currentQuizList.Count * 100;
        scoreText.text = string.Format("{0}",(int)score) + "점";
        this.score = (int)score;
        if (score > 80)
        {
            StartCoroutine(PlayStars(3));
        }
        else if (score > 50)
        {
            StartCoroutine(PlayStars(2));
        }
        else if(score > 30)
        {
            StartCoroutine(PlayStars(1));
        }
        else
        {
            StartCoroutine(PlayStars(0));
        }
        if (score > 50)
        {
            audioSource.clip = completeSound;
        }
        else
        {
            audioSource.clip = completeFailSound;
        }
        audioSource.Play();
    }
    public void OnBookmarkClicked()
    {
        if(BookmarkComponent.AddBookmark(currentQuizList[currentIndex]))
        {
            UpdateBookmarkBtnVisibleState();
        }
    }
    public override void Show()
    {
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
    }
    public override void OnBackEvent()
    {
        if (backBlock)//게임 완료 시에는 Block
            return;

        UIManager.Ins.PopPanel();
    }
    public void OnCompleteBtnClicked()
    {
        if (stage > 0)
        {
            bool bScoreChange = BestScoreComponent.GetScore(stage) < score;
            BestScoreComponent.UpdateBestScore(stage, score);
            onTestCompleted.Invoke(stage, score, bScoreChange);
            //플레이 시간 기록
            MyPlayDataComponent.UpdatePlayTime(completeTime);
            //정답률 기록
            MyPlayDataComponent.UpdateAnswerRate(correctCount, currentQuizList.Count);
        }
        UIManager.Ins.PopPanel();
    }

    IEnumerator PlayStars(int count)
    {
        completeBtn.enabled = false;
        backBlock = true;
        if (count >= 1)
        {
            stars[0].PlayAnimation();
            yield return new WaitForSeconds(1);
        }
        else
        {
            stars[0].PlayDisableAnimation();
            yield return new WaitForSeconds(0.5f);
        }
        

        if (count >= 2)
        {
            stars[1].PlayAnimation();
            yield return new WaitForSeconds(1);
        }
        else
        {
            stars[1].PlayDisableAnimation();
            yield return new WaitForSeconds(0.5f);
        }
        

        if (count >= 3)
        {
            stars[2].PlayAnimation();
            yield return new WaitForSeconds(1);
        }
        else
        {
            stars[2].PlayDisableAnimation();
        }
        
        completeBtn.enabled = true;
        backBlock = false;
    }

    public override string GetPanelName()
    {
        return "InGamePanel";
    }
}