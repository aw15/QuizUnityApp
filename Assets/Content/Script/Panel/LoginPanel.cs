using System;
using System.Collections;
using Content.Script.Manager;
using Content.Script.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class LoginPanel : IPanel
{
    [SerializeField]
    UltimateClean.CleanButton loginBtn;

    [SerializeField] private CleanBtnColorScript loginBtnColorScript;

    [SerializeField] private Image centerImage1;
    [SerializeField] private Image centerImage2;
    [SerializeField] private Image centerImage3;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private AdManager adManager;

    private bool isFirstLogin = true;
    private void Start()
    {
        UIManager.Ins.PushPanel("LoginPanel");

        loginBtn.onClick.AddListener(OnLoginBtnClicked);
        
        //AppSetting
        titleText.text = AppTypeManager.AppSetting.title;
        centerImage1.sprite = AppTypeManager.AppSetting.titleSprites[0];
        centerImage2.sprite = AppTypeManager.AppSetting.titleSprites[1];
        centerImage3.sprite = AppTypeManager.AppSetting.titleSprites[2];
    }
    private void OnDestroy()
    {
        loginBtn.onClick.RemoveListener(OnLoginBtnClicked);
    }
    public void OnFindPWBtnClicked()
    {
        UIManager.Ins.PushPanel("FindPasswordPanel");
    }
    public void OnLoginBtnClicked()
    {
        if (isFirstLogin)
        {
            loginBtn.interactable = false;
            loginBtnColorScript.SetColor(Color.gray);
            isFirstLogin = false;
#if UNITY_EDITOR
        StartCoroutine(WaitForAdLoaded(0));    
#else
        StartCoroutine(WaitForAdLoaded(0.5f));
#endif
        }
        else
        {
            UIManager.Ins.PushPanel("GameSettingPanel");
        }
    }
    
    IEnumerator WaitForAdLoaded(float second)
    {
        yield return new WaitForSeconds(second);
        // Get the Ad Unit ID for the current platform:
        adManager.ShowBannerAd();
        DataManager.Ins.DatabaseInit();  
    }
    
    public void OnDatabaseLoaded()
    {
        loginBtn.interactable = true;
        loginBtnColorScript.SetDefaultColor();
        UIManager.Ins.PushPanel("GameSettingPanel");
    }
    public void OnDatabaseLoadFailed()
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
