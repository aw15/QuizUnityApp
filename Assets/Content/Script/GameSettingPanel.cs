using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingPanel : IPanel
{
    private const string containAlreadyQuizKey = "containAlreadyQuiz";
    public TMP_InputField searchInputField;
    public TextMeshProUGUI candidataText;
    public GameObject philosopherPref;
    public GameObject gridLayout;
    public Toggle containAlreadyQuizToggle;
    private void Start()
    {
        UIManager.Ins.AddPanel("GameSettingPanel", this);
        containAlreadyQuizToggle.isOn = PlayerPrefs.GetInt(containAlreadyQuizKey, 0) == 0 ? false : true;
        DataManager.Ins.isContainAlreadySolved = containAlreadyQuizToggle.isOn;
        containAlreadyQuizToggle.onValueChanged.AddListener(OnContainAlreadyQuizToggleChanged);
        Hide();
    }
    private void OnDestroy()
    {
        containAlreadyQuizToggle.onValueChanged.RemoveAllListeners();
    }
    void Clear()
    {

    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        UIManager.Ins.AddPanel("GameSettingPanel", this);
    }
# endif
    void OnContainAlreadyQuizToggleChanged(bool isOn)
    {
        DataManager.Ins.isContainAlreadySolved = isOn;
        PlayerPrefs.SetInt(containAlreadyQuizKey, isOn ? 1 : 0);
    }
    public void OnDataLoaded() // ¡¯¿‘¡°.
    {
        Clear();
        foreach (Transform child in gridLayout.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject newObj;
        foreach(var philosopher in DataManager.Ins.philosopherList.data)
        {
            if (DataManager.Ins.quizDatabase.data.ContainsKey(philosopher) == false || DataManager.Ins.quizDatabase.data[philosopher].Count <= 0)
            {
                continue;
            }

            newObj = (GameObject)Instantiate(philosopherPref, gridLayout.transform);
            var btnScriptRef = newObj.GetComponentInChildren<PhilosopherButton>();
            btnScriptRef.categoryName = philosopher;
            btnScriptRef.color = btnScriptRef.RandomColor();
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
    public void OnBackBtnClick()
    {
        UIManager.Ins.PopPanel();
    }
}
