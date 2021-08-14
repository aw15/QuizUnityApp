using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.UI;

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
    public void OnDataLoaded() // ¡¯¿‘¡°.
    {
        Clear();
        adManager.Show();
        foreach (Transform child in gridLayout.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject newObj;
        int count = 1;
        foreach (var philosopher in DataManager.Ins.philosopherList.data)
        {
            if (DataManager.Ins.quizDatabase.data.ContainsKey(philosopher) == false || DataManager.Ins.quizDatabase.data[philosopher].Count <= 0)
            {
                continue;
            }

            newObj = (GameObject)Instantiate(philosopherPref, gridLayout.transform);
            var btnScriptRef = newObj.GetComponentInChildren<PhilosopherButton>();
            btnScriptRef.categoryName = philosopher;
            btnScriptRef.number = count.ToString();
            count += 1;
        }
    }

    public void OnSearchPhilosopherChanged()
    {
        candidataText.text = string.Empty;

        string input = searchInputField.text;
        if (string.IsNullOrEmpty(input))
            return;

        var pattern = Utils.HangulSearchPattern(input);
        var candidataList = DataManager.Ins.philosopherList.data.Where(e => Regex.IsMatch(e.ToString(), pattern));

        foreach (var text in candidataList)
        {
            candidataText.text += text + "\r\n";
        }
    }

    public void OnSearchPhilosopherBtnClick()
    {
        int index = DataManager.Ins.philosopherList.data.FindIndex((param) => { return param.Equals(searchInputField.text, System.StringComparison.OrdinalIgnoreCase); });
        Debug.Log(index);
        if (index < 0)
            return;
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
