using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class LoginPanel : IPanel
{
    private const string rememberLoginInfoKey = "RememberLoginInfo";
    private const string emailKey = "Email";
    private const string passwordKey = "Password";

    [SerializeField]
    UltimateClean.CleanButton loginBtn;
    [SerializeField]
    UltimateClean.CleanButton registerBtn;
    [SerializeField]
    UltimateClean.CleanButton findPwBtn;
    [SerializeField]
    UltimateClean.CleanButton copyrightBtn;
    [SerializeField]
    Toggle rememberToggle;
    [SerializeField]
    TMP_InputField emailUI;
    [SerializeField]
    TMP_InputField pwUI;
    private void Start()
    {
        UIManager.Ins.PushPanel("LoginPanel");

        rememberToggle.isOn = PlayerPrefs.GetInt(rememberLoginInfoKey, 0) == 0 ? false : true;
        if (rememberToggle.isOn)
        {
            emailUI.text = PlayerPrefs.GetString(emailKey, string.Empty);
            pwUI.text = PlayerPrefs.GetString(passwordKey, string.Empty);
        }
        rememberToggle.onValueChanged.AddListener(OnRememberToggleChanged);
        loginBtn.onClick.AddListener(OnLoginBtnClicked);
        registerBtn.onClick.AddListener(OnRegisterBtnClicked);
        findPwBtn.onClick.AddListener(OnFindPWBtnClicked);
        copyrightBtn.onClick.AddListener(OnCopyrightBtnClicked);
    }
    private void OnDestroy()
    {
        loginBtn.onClick.RemoveListener(OnLoginBtnClicked);
        registerBtn.onClick.RemoveListener(OnRegisterBtnClicked);
        findPwBtn.onClick.RemoveListener(OnFindPWBtnClicked);
        copyrightBtn.onClick.RemoveListener(OnCopyrightBtnClicked);
    }
    public void OnCopyrightBtnClicked()
    {
        UIManager.Ins.PushPanel("CopyrightPanel");
    }
    public void OnFindPWBtnClicked()
    {
        UIManager.Ins.PushPanel("FindPasswordPanel");
    }
    public void OnLoginBtnClicked()
    {
        loginBtn.interactable = false;
        if(rememberToggle.isOn)
        {
            PlayerPrefs.SetString(emailKey, emailUI.text);
            PlayerPrefs.SetString(passwordKey, pwUI.text);
        }
        //AuthProcess.Ins.Login(emailUI.text, pwUI.text); 로그인 기능 비활성화
        OnLoginComplete(true, "");
    }
    public void OnLoginComplete(bool result, string reason)
    {
        if (result == false)
        {
            loginBtn.interactable = true;
            return;
        }

        DataManager.Ins.DatabaseInit();
    }
    public void OnDatabaseLoaded()
    {
        UIManager.Ins.PushPanel("GameSettingPanel");
    }
    public void OnRememberToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(rememberLoginInfoKey, isOn ? 1 : 0);
    }
    public void OnRegisterBtnClicked()
    {
        UIManager.Ins.panelStack.Push(UIManager.Ins.GetPanel("RegisterPanel"));
    }
    public void OnFirebaseInit()
    {
        loginBtn.interactable = true;
    }
    public override void Show()
    {
        loginBtn.interactable = true;
        base.Show();
    }
    public override void OnBackEvent()
    {
        Application.Quit();
    }
    public override string GetPanelName()
    {
        return "LoginPanel";
    }
}
