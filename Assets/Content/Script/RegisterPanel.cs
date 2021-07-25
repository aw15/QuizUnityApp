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
        UIManager.Ins.AddPanel("RegisterPanel", this);
        Hide();
    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        UIManager.Ins.AddPanel("RegisterPanel", this);
    }
# endif
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
        UIManager.panelStack.Pop();
    }
}
