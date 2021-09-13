using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;

public class GameSettingPanel : IPanel
{
    private const string containAlreadyQuizKey = "containAlreadyQuiz";
    public TMP_InputField searchInputField;
    public TextMeshProUGUI candidataText;
    public GameObject philosopherPref;
    public GameObject gridLayout;
    public AdManager adManager;
    [SerializeField]
    CleanButton bookmarkBtn;
    [SerializeField]
    CleanButton settingBtn;
    [SerializeField]
    CleanButton profileBtn;
    List<StageButtonScript> stageButtonRefs = new List<StageButtonScript>();
    
    private void Start()
    {
        Hide();
        bookmarkBtn.onClick.AddListener(OnBookMarkBtnClicked);
        settingBtn.onClick.AddListener(OnSettingBtnClicked);
        profileBtn.onClick.AddListener(OnProfileBtnClicked);
    }
    private void OnDestroy()
    {
        bookmarkBtn.onClick.RemoveListener(OnBookMarkBtnClicked);
        settingBtn.onClick.RemoveListener(OnSettingBtnClicked);
        profileBtn.onClick.RemoveListener(OnProfileBtnClicked);
    }
    void Clear()
    {

    }
    public void OnProfileBtnClicked()
    {
        UIManager.Ins.PushPanel("ProfilePanel");
    }
    public void OnBookMarkBtnClicked()
    {
        UIManager.Ins.PushPanel("BookmarkPanel");
    }
    public void OnSettingBtnClicked()
    {
        UIManager.Ins.PushPanel("SettingPanel");
    }
    public void OnDataLoaded() // ÁøÀÔÁ¡.
    {
        Clear();
        adManager.Show();
        foreach (Transform child in gridLayout.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        stageButtonRefs.Clear();

        GameObject newObj;

        int count = 1;
        for (int index = 0; ; index += CommonDefines.stageQuizCount)
        {
            if (index >= DataManager.Ins.quizDatabase.Data.Count)
                break;

            newObj = (GameObject)Instantiate(philosopherPref, gridLayout.transform);
            var btnScriptRef = newObj.GetComponentInChildren<StageButtonScript>();
            btnScriptRef.number = count.ToString();
            btnScriptRef.startIndex = index;
            btnScriptRef.stage = count;
            if (index + (CommonDefines.stageQuizCount-1) >= DataManager.Ins.quizDatabase.Data.Count)
            {
                btnScriptRef.lastIndex = DataManager.Ins.quizDatabase.Data.Count - 1;
            }
            else
            {
                btnScriptRef.lastIndex = index + (CommonDefines.stageQuizCount - 1);
            }
            btnScriptRef.bestScore = BestScoreComponent.GetScore(count).ToString();
            stageButtonRefs.Add(btnScriptRef);
            count += 1;
        }
    }
    public void OnTestCompleted(int stage, int score, bool bScoreChange)
    {
        if(stage - 1 < stageButtonRefs.Count && bScoreChange)
        {
            stageButtonRefs[stage - 1].bestScore = score.ToString();
        }
    }
    public override void OnBackEvent()
    {
        UIManager.Ins.PopPanel();
        adManager.Hide();
    }
    public override string GetPanelName()
    {
        return "GameSettingPanel";
    }
}
