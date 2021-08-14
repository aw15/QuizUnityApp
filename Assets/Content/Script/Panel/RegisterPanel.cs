using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPanel : IPanel
{
    public UltimateClean.CleanButton registerButtonRef;
    public string email { get; set; } = string.Empty;
    public string pw { get; set; } = string.Empty;
    public string pwConfirm { get; set; } = string.Empty;
    private void Start()
    {
        Hide();
        registerButtonRef.onClick.AddListener(OnRegisterBtnClicked);
    }
    private void OnDestroy()
    {
        registerButtonRef.onClick.RemoveListener(OnRegisterBtnClicked);
    }
    public override void OnBackEvent()
    {
        UIManager.Ins.PopPanel();
    }
    public void OnRegisterBtnClicked()
    {
        if (false == Utils.IsValidEmail(email))
        {
            UIManager.Ins.ShowNotification("���� ��� ����", "�ùٸ��� �ʴ� �̸����Դϴ�.", Notification.InfoType.Error);
            return;
        }

        if (pw != pwConfirm)
        {
            UIManager.Ins.ShowNotification("���� ��� ����", "��� ��ȣ�� ��ġ���� �ʽ��ϴ�.", Notification.InfoType.Error);
            return;
        }

        registerButtonRef.interactable = false;
        AuthProcess.Ins.Register(email, pw);
    }
    public void OnRegiterationComplete(bool result)
    {
        registerButtonRef.interactable = true;
        UIManager.Ins.panelStack.Pop();
    }
    public override string GetPanelName()
    {
        return "RegisterPanel";
    }
}
