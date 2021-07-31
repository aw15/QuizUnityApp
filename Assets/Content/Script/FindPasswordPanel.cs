using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindPasswordPanel : IPanel
{
    [SerializeField]
    UltimateClean.CleanButton findPwBtn;
    [SerializeField]
    UltimateClean.CleanButton backBtn;
    [SerializeField]
    TMP_InputField emailUI;
    [SerializeField]
    TextMeshProUGUI descriptionUI;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Ins.AddPanel("FindPasswordPanel", this);
        Hide();

        descriptionUI.enabled = false;
        findPwBtn.onClick.AddListener(OnFindPasswordBtnClicked);
    }
    private void OnDestroy()
    {
        findPwBtn.onClick.RemoveListener(OnFindPasswordBtnClicked);
    }
    async void OnFindPasswordBtnClicked()
    {
        findPwBtn.interactable = false;
        backBtn.interactable = false;
        descriptionUI.enabled = false;
        bool success = await AuthProcess.Ins.FindPassword(emailUI.text);
        descriptionUI.enabled = true;
        if(success)
        {
            descriptionUI.text = "재설정 메일이 전송되었습니다.";
        }
        else
        {
            descriptionUI.text = "재설정 메일이 전송에 실패하였습니다. 문의바랍니다.";
        }
        backBtn.interactable = true;
        findPwBtn.interactable = true;
    }
    public void OnOpen()
    {
        panelRef.SetActive(true);
        descriptionUI.enabled = false;
    }
    public void OnClose()
    {
        panelRef.SetActive(false);
    }
}
