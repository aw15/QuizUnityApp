using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Content.Script
{
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
        Toggle rememberToggle;
        [SerializeField]
        TMP_InputField emailUI;
        [SerializeField]
        TMP_InputField pwUI;
        private void Start()
        {
            UIManager.Ins.AddPanel("LoginPanel", this);
            UIManager.Ins.PushPanel("LoginPanel");
            rememberToggle.onValueChanged.AddListener(OnRememberToggleChanged);
            rememberToggle.isOn = PlayerPrefs.GetInt(rememberLoginInfoKey, 0) == 0 ? false : true;
            if (rememberToggle.isOn)
            {
                emailUI.text = PlayerPrefs.GetString(emailKey, string.Empty);
                pwUI.text = PlayerPrefs.GetString(passwordKey, string.Empty);
            }
        }
#if UNITY_EDITOR
        private void OnEnable()
        {
            UIManager.Ins.AddPanel("LoginPanel", this);
        }
# endif
        public void OnLoginBtnClicked()
        {
            loginBtn.interactable = false;
            if(rememberToggle.isOn)
            {
                PlayerPrefs.SetString(emailKey, emailUI.text);
                PlayerPrefs.SetString(passwordKey, pwUI.text);
            }
            AuthProcess.Ins.Login(emailUI.text, pwUI.text); 
        }
        public void OnLoginComplete(bool result, string reason)
        {
            loginBtn.interactable = true;
            if (result == false)
                return;

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
            UIManager.panelStack.Push(UIManager.Ins.GetPanel("RegisterPanel"));
        }
        public void OnFirebaseInit()
        {
            loginBtn.interactable = true;
        }
        public override void Show()
        {
            base.Show();
        }
    }
}
