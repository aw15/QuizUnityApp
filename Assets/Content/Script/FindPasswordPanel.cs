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
            descriptionUI.text = "�缳�� ������ ���۵Ǿ����ϴ�.";
        }
        else
        {
            descriptionUI.text = "�缳�� ������ ���ۿ� �����Ͽ����ϴ�. ���ǹٶ��ϴ�.";
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
